using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ValuedInBETests.TestingConstants;
using ValuedInBE.Users.Services;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Models;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Chats.EventHandlers;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.DTOs.Response;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Chats.Models.Events;
using ValuedInBE.Chats.Repositories;
using ValuedInBE.Chats.Services.Implementations;
using ValuedInBE.DataControls.Paging;

namespace ValuedInBE.Services.Chats.Implementations.Tests
{
    public class ChatServiceTests
    {
        private readonly Mock<IChatRepository> _chatRepositoryMock = new();
        private readonly Mock<ILogger<ChatService>> _loggerMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IHttpContextAccessor> _contextAccessorMock = new();
        private readonly Mock<IMessageEventHandler> _messageEventHandlerMock = new();
        private readonly Mock<HttpContext> _httpContextMock = new();

        private ChatService MockChatService()
        {
            UserContext userContext = UserConstants.UserContextInstance;
            _httpContextMock.SetupGet(context => context.Items)
                .Returns(new Dictionary<object, object?>() { { UserContextMiddleware.userContextItemName, userContext } });
            _contextAccessorMock.SetupGet(accessor => accessor.HttpContext)
                .Returns(_httpContextMock.Object);
            return new(_chatRepositoryMock.Object, _loggerMock.Object, _contextAccessorMock.Object, _messageEventHandlerMock.Object, _userServiceMock.Object);
        }

        [Fact()]
        public async void TestGetChatsAsync()
        {
            DateTime lowerDateTime = DateTime.Now + TimeSpan.FromDays(2);
            DateTime higherDateTime = DateTime.Now;
            TimeSpan comparisonRange = TimeSpan.FromMinutes(1);
            string userId = UserConstants.userId;
            int size = 10;
            List<ChatMessage> messages = new()
            {
                new ChatMessage{ ChatId = 0, CreatedOn = lowerDateTime, Message = "Last" },
                new ChatMessage{ ChatId = 0, CreatedOn= higherDateTime, Message = "First" },
            };
            ChatParticipant chatParticipant1 = new()
            {
                ChatId = 1,
                UserId = userId,
            };
            ChatParticipant chatParticipant2 = new()
            {
                ChatId = 2,
                UserId = userId,
            };
            List<Chat> chats = new()
            {
                new Chat
                {
                    Id = 1, 
                    CreatedOn = DateTime.Now, 
                    Messages = messages.Select(mess => { mess.ChatId = 1; return mess; }).OrderBy(mess=>mess.CreatedOn).ToList(),
                    Participants = new(){chatParticipant1}
                },
                new Chat
                {
                    Id = 2,
                    CreatedOn = lowerDateTime,
                    Messages = messages.Select(mess => { mess.ChatId = 2; return mess; }).OrderByDescending(mess=>mess.CreatedOn).ToList(),
                    Participants = new(){chatParticipant1}
                },
            };

            ChatPageRequest pageRequest = new()
            {
                Offset = lowerDateTime,
                Size = size,
            };

            _chatRepositoryMock.Setup(repository => repository.GetChatsWithLastMessageAndParticipantsAsync(userId, size, lowerDateTime))
                .ReturnsAsync(chats);

            ChatService service = MockChatService();
            OffsetPage<ChatInfo, DateTime> offsetPage = await service.GetChatsAsync(pageRequest);

            Assert.True(offsetPage.Last);
            Assert.Equal(offsetPage.NextOffset, lowerDateTime);

            IEnumerable<ChatInfo> chatInfos = offsetPage.Results;
            Assert.True(chatInfos.Any());
            Assert.Equal(2, chatInfos.Count());
            Assert.Equal(higherDateTime, chatInfos.First().LastMessage, comparisonRange);
            Assert.Equal("First", chatInfos.First().LastMessageContent);
            Assert.Equal(userId, chatInfos.First().ParticipatingUsers.First());
            Assert.Equal(lowerDateTime, chatInfos.Last().LastMessage, comparisonRange);
            Assert.Equal("Last", chatInfos.Last().LastMessageContent);
            Assert.Equal(userId, chatInfos.Last().ParticipatingUsers.First());
        }

        [Fact()]
        public async Task TestFetchOrCreateChatAsyncWhenChatExistsAsync()
        {
            string userId = UserConstants.userId;
            string receivedId = "receiver";
            List<string> participantIds = new() { receivedId };
            NewChatRequest newChatRequest = new()
            {
                MessageContent = "message",
                Participants = participantIds
            };
            Chat chat = new()
            {
                Id = 1,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Messages = new(),
                Participants = new()
            };

            _userServiceMock.Setup(service => service.VerifyUserIdExistenceAsync(participantIds))
                .ReturnsAsync(true);

            _chatRepositoryMock.Setup(repository => repository.GetChatFromParticipantsAsync(It.Is<List<string>>(list => list.Contains(receivedId) && list.Contains(userId))))
                .ReturnsAsync(chat);

            ChatService service = MockChatService();
            Chat receivedChat = await service.FetchOrCreateChatAsync(newChatRequest);
            Assert.Equal(chat, receivedChat);
        }

        [Fact()]
        public async void TestFetchOrCreateChatAsyncWhenChatNotExists()
        {
            string userId = UserConstants.userId;
            string receiverId = "receiver";
            string message = "message";
            long chatId = ChatConstants.chatID;
            List<string> participantIds = new() { receiverId };
            NewChatRequest newChatRequest = new()
            {
                MessageContent = message,
                Participants = participantIds
            };

            _userServiceMock.Setup(service => service.VerifyUserIdExistenceAsync(participantIds))
                .ReturnsAsync(true);
            _chatRepositoryMock.Setup(repository => repository.GetChatFromParticipantsAsync(It.Is<List<string>>(list => list.Contains(receiverId) && list.Contains(userId))))
                .ReturnsAsync((Chat?)null);

            _chatRepositoryMock.Setup(repository => repository.SaveNewChatAsync(It.IsAny<Chat>()))
                .Callback<Chat>(chat => { chat.Id = chatId; });

            _chatRepositoryMock.Setup(repository => repository.AddChatParticipantsAsync(It.Is<List<ChatParticipant>>(list => list.All(participant => participant.ChatId == chatId))))
                .Verifiable();

            _chatRepositoryMock.Setup(repository => repository.CreateNewChatMessageAsync(It.Is<ChatMessage>(message => message.ChatId == chatId && message.Message.Equals(message))))
                .Verifiable();

            _chatRepositoryMock.Setup(repository => repository.GetParticipantIdsFromChat(chatId))
                .ReturnsAsync(participantIds);

            _messageEventHandlerMock.Setup(handler => handler.HandleSentMessageEvent(It.Is<NewMessageEvent>(message => message.OtherParticipantIDs == participantIds && message.ChatMessage.ChatId == chatId)))
                .Verifiable();

            ChatService service = MockChatService();
            Chat chat = await service.FetchOrCreateChatAsync(newChatRequest);
            Mock.Verify();
            Assert.NotNull(chat);
            Assert.Equal(chatId, chat.Id);
            Assert.Null(chat.Participants);
            Assert.Null(chat.Messages);
        }


        [Fact()]
        public async void TestGetMessagesAsync()
        {
            long chatId = ChatConstants.chatID;
            string userId = UserConstants.userId;
            UserDetails user = UserConstants.UserDetailsInstance;
            DateTime dateTimeOffset = DateTime.Now;
            DateTime newDateTimeOffset = DateTime.Now.AddDays(1);
            int size = 10;
            string message = "message";
            MessagePageRequest pageRequest = new()
            {
                Offset = dateTimeOffset,
                Size = size,
            };
            Chat chat = new()
            {
                Id = chatId,
                CreatedBy = userId,
                CreatedOn = dateTimeOffset,
                Messages = new() { new ChatMessage() {Id = 1, ChatId = chatId, CreatedOn = newDateTimeOffset, CreatedBy = userId, Message = message } },
                Participants = new() { new(){ ChatId = chatId,  UserId = userId, User = user } }
            };
            _chatRepositoryMock.Setup(repository => repository.GetChatMessagesWithParticipantsDetails(chatId, size, dateTimeOffset))
                .ReturnsAsync(chat);

            ChatService chatService = MockChatService();
            OffsetPage<MessageDTO, DateTime> offsetPage = await chatService.GetMessagesAsync(pageRequest, chatId);
            Assert.NotNull(offsetPage);
            Assert.Equal(newDateTimeOffset, offsetPage.NextOffset);
            Assert.True(offsetPage.Last);
            Assert.Single(offsetPage.Results);
            MessageDTO receivedMessage = offsetPage.Results.First();
            Assert.Equal(1, receivedMessage.Id);
            Assert.Equal(user.FirstName, receivedMessage.SentByFirstName);
            Assert.Equal(user.LastName, receivedMessage.SentByLastName);
            Assert.Equal(message, receivedMessage.Content);
            Assert.Equal(newDateTimeOffset, receivedMessage.Sent);
        }
    }
}