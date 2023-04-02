
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using ValuedInBE;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Users.Models.Entities;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests.Chats
{
    public class ChattingIntegrationTests : IntegrationTestBase
    {
        private const string chatsRoute = "/api/chats";
        private const string singleChatRoute = "/api/chats/{id}";
        private const string webSocketRoute = "/ws";
        private const string webSocketTokenRoute = "/ws/token";
        private const string senderLogin = "ChatMessageSender";
        private const string receiverLogin = "ChatMessageReceiver";
        private const string sendableMessage = "HELLO THIS IS NEW MESSAGE";
        private const string pingMessage = "ping";
        private readonly WebSocketClient _webSocketClient; 

        public ChattingIntegrationTests(IntegrationTestWebApplicationFactory<Program> factory) : base(factory)
        {
            _webSocketClient = factory.Server.CreateWebSocketClient();
        }

        [Fact]
        public async Task CreateChatAndThenTryToCreateTheSameChat() 
        {
            AddLoginHeaderToHttpClient(senderLogin);
            string receiver = await GetUserIdFromLoginAsync(receiverLogin); 

            NewChatRequest request = new()
            {
                MessageContent = sendableMessage,
                Participants = new() { receiver }
            };
            StringContent content = SerializeIntoJsonHttpContent(request);

            HttpResponseMessage response = await _client.PostAsync(chatsRoute, content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            string initialChatResponse = await response.Content.ReadAsStringAsync();
            Chat? initialChat = JsonConvert.DeserializeObject<Chat>(initialChatResponse);
            Assert.NotNull(initialChat);
            //repeat for a second attempt, should receive same Id
            response = await _client.PostAsync(chatsRoute, content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            string secondaryChatResponse = await response.Content.ReadAsStringAsync();
            Chat? secondaryChat = JsonConvert.DeserializeObject<Chat>(secondaryChatResponse);
            Assert.NotNull(secondaryChat);
            Assert.Equal(initialChat!.Id, secondaryChat!.Id);
        }

        [Fact]
        public async Task CreatingChatAndSendingAMessageShouldBothTriggerWebSocket()
        {
            await Task.Delay(10000);
            string messageContent = "This is message content";
            string secondMessageContent = "This is different content";
            string receiver = await GetUserIdFromLoginAsync(receiverLogin);
            string sender = await GetUserIdFromLoginAsync(senderLogin);
            byte[] buffer = new byte[1024 * 4];
            ArraySegment<byte> socketResponse = new(buffer);
            NewMessage newMessage = new() { Content = secondMessageContent };
            NewChatRequest request = new()
            {
                MessageContent = messageContent,
                Participants = new() { receiver }
            };
            StringContent chatRequestContent = SerializeIntoJsonHttpContent(request);
            AddLoginHeaderToHttpClient(senderLogin);
            WebSocket webSocket = await EstablishWebSocketConnection();

            HttpResponseMessage chatResponse = await _client.PostAsync(chatsRoute, chatRequestContent);

            var WebSocketReceiveTask = webSocket.ReceiveAsync(socketResponse, CancellationToken.None);
            Assert.True(chatResponse.IsSuccessStatusCode);
            Assert.NotNull(chatResponse.Content);
            string chatResponseString = await chatResponse.Content.ReadAsStringAsync();
            Chat? createdChat = JsonConvert.DeserializeObject<Chat>(chatResponseString);
            Assert.NotNull(createdChat);
            WebSocketReceiveResult firstResults = await WebSocketReceiveTask;
            Assert.False(firstResults.CloseStatus.HasValue, "Should not have closed the socekt");
            Assert.NotNull(socketResponse.Array);
            string socketResponseAsString = Encoding.UTF8.GetString(socketResponse.Array!).Trim('\0');
            ChatMessage? chatMessage = JsonConvert.DeserializeObject<ChatMessage>(socketResponseAsString);

            Assert.NotNull(chatMessage);
            Assert.Equal(chatMessage!.CreatedBy, sender);
            Assert.Equal(chatMessage.Message, messageContent);
            Assert.Equal(chatMessage.ChatId, createdChat!.Id);

            string specificMessageRoute = singleChatRoute.Replace("{id}", createdChat.Id.ToString());
            StringContent messageRequestContent = SerializeIntoJsonHttpContent(newMessage);

            HttpResponseMessage messageResponse = await _client.PostAsync(specificMessageRoute, messageRequestContent);
            Assert.True(messageResponse.IsSuccessStatusCode, messageResponse.StatusCode.ToString());
            Assert.NotNull(messageResponse.Content);

            byte[] buffer3 = new byte[1024 * 4];
            socketResponse = new(buffer3);
            WebSocketReceiveResult results = await webSocket.ReceiveAsync(socketResponse, CancellationToken.None);
            Assert.False(results.CloseStatus.HasValue, "Should not have closed the socket 2");
            Assert.NotNull(socketResponse.Array);
            string secondSocketResponseAsString = Encoding.UTF8.GetString(socketResponse.Array!);
            ChatMessage? secondChatMessage = JsonConvert.DeserializeObject<ChatMessage>(secondSocketResponseAsString);
            Assert.NotNull(secondChatMessage);
            Assert.Equal(secondChatMessage!.CreatedBy, sender);
            Assert.Equal(secondChatMessage.Message, secondMessageContent);
            Assert.Equal(secondChatMessage.ChatId, createdChat.Id);

            await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }

        private async Task<string> GetUserIdFromLoginAsync(string login)
        {
            UserCredentials user = await _valuedInContext.UserCredentials.FirstAsync(c => c.Login == login);
            return user.UserID;
        }

        private async Task CheckWebSocketConnection(WebSocket webSocket)
        {
            Assert.False(webSocket.CloseStatus.HasValue);
            ArraySegment<byte> buffer = new(Encoding.UTF8.GetBytes(pingMessage));
            byte[] buffer2 = new byte[1024 * 4];
            ArraySegment<byte> socketResponse = new(buffer2);
            WebSocketReceiveResult firstResults = await webSocket.ReceiveAsync(socketResponse, CancellationToken.None);
            Assert.False(firstResults.CloseStatus.HasValue);
            Assert.NotNull(socketResponse.Array);
            Assert.Equal(pingMessage, Encoding.UTF8.GetString(socketResponse.Array!).Trim('\0'));
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Assert.False(firstResults.CloseStatus.HasValue);
        }

        private async Task<WebSocket> EstablishWebSocketConnection()
        {
            string webSocketToken = await GetTokenForWebSocket();
            Uri webSocketUri = new UriBuilder(_clientBaseAdress) { Scheme = "wss", Path = webSocketRoute, Query = $"token={webSocketToken}" }.Uri;
            WebSocket webSocket = await _webSocketClient.ConnectAsync(webSocketUri, CancellationToken.None);
            await CheckWebSocketConnection(webSocket);
            return webSocket;
        }

        private async Task<string> GetTokenForWebSocket()
        {
            HttpResponseMessage tokenResponse = await _client.GetAsync(webSocketTokenRoute);
            Assert.True(tokenResponse.IsSuccessStatusCode);
            Assert.NotNull(tokenResponse.Content);
            string webSocketToken = await tokenResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(webSocketToken));
            return webSocketToken;
        }
    }
}
