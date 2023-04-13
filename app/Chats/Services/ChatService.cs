using ValuedInBE.Chats.EventHandlers;
using ValuedInBE.Chats.Exceptions;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.DTOs.Response;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Chats.Models.Events;
using ValuedInBE.Chats.Repositories;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Services;

namespace ValuedInBE.Chats.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly ILogger<ChatService> _logger;
        private readonly IMessageEventHandler _eventHandler;
        private readonly IUserContextAccessor _userContextAccessor;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger, IMessageEventHandler eventHandler, IUserService userService, IUserContextAccessor userContextAccessor)
        {
            _chatRepository = chatRepository;
            _logger = logger;
            _eventHandler = eventHandler;
            _userService = userService;
            _userContextAccessor = userContextAccessor;
        }

        public async Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage)
        {
            DateTime? createdSince = chatPage.Offset;
            UserContext userContext = _userContextAccessor.UserContext ?? throw new Exception("User Context not found");

            _logger.LogDebug("User {user} has requested Chat Page with Offset {offset}", userContext.UserID, createdSince);
            IEnumerable<Chat> chats = await _chatRepository.GetChatsWithLastMessageAndParticipantsAsync(userContext.UserID, chatPage.Size, chatPage.Offset);
            DateTime newOffset = chats.LastOrDefault()?.CreatedOn ?? createdSince ?? DateTime.Now;
            bool isLast = chats.Count() != chatPage.Size; //Should never be more; if less, means there's no more left

            _logger.LogTrace("Fetched {count} chat records, setting new offset to {newOffset}. {lastPageMessage}", chats.Count(), newOffset, isLast ? "Is last page" : "Is not last page ");

            IEnumerable<ChatInfo> chatInfos =
                chats.Select(
                chat => new ChatInfo
                {
                    Id = chat.Id,
                    ChatName = "Lorem Ipsum", //TODO: this is not represented in DB model, need to address with team
                    LastMessage = chat.Messages.First().CreatedOn,
                    LastMessageContent = chat.Messages.First().Message,
                    ParticipatingUsers = chat.Participants.Select(p => p.UserId).ToList(),
                    Unread = true
                });

            return new()
            {
                Last = isLast,
                NextOffset = newOffset,
                Results = chatInfos,
            };
        }

        public async Task<Chat> FetchOrCreateChatAsync(NewChatRequest newChatRequest)
        {
            UserContext userContext = _userContextAccessor.UserContext ?? throw new Exception("User Context not found");
            if (newChatRequest.Participants.Count == (newChatRequest.Participants.Contains(userContext.UserID) ? 1 : 0))
                throw new ChatParticipantsMissingException("No participants");

            Task<bool> usersExistTask = _userService.VerifyUserIdExistenceAsync(newChatRequest.Participants);

            if (!await usersExistTask)
            {
                throw new ChatParticipantsMissingException("Not all participants exist");
            }

            List<string> allParticipants = newChatRequest.Participants;
            allParticipants.Add(userContext.UserID);

            Chat? existingChat = await _chatRepository.GetChatFromParticipantsAsync(allParticipants);

            if (existingChat != null) return existingChat;

            Chat newChat = new();
            await _chatRepository.SaveNewChatAsync(newChat);
            long newChatId = newChat.Id;

            List<ChatParticipant> chatParticipants =
                allParticipants.Select(
                    p => new ChatParticipant()
                    {
                        ChatId = newChatId,
                        UserId = p
                    }
                ).ToList();

            await _chatRepository.AddChatParticipantsAsync(chatParticipants);

            await CreateNewMessageAsync(newChatId, newChatRequest.MessageContent);

            return newChat;
        }

        public async Task<ChatMessage> CreateNewMessageAsync(long chatId, string message)
        {
            ChatMessage chatMessage = new()
            {
                ChatId = chatId,
                Message = message
            };
            await _chatRepository.CreateNewChatMessageAsync(chatMessage);

            await SendMessageEvent(chatMessage);
            return chatMessage;
        }

        public async Task SendMessageEvent(ChatMessage chatMessage, IEnumerable<string>? otherParticipants = null)
        {
            otherParticipants ??= await _chatRepository.GetParticipantIdsFromChatAsync(chatMessage.ChatId);
            NewMessageEvent messageEvent = new()
            {
                ChatMessage = chatMessage,
                OtherParticipantIDs = otherParticipants,
            };

            await _eventHandler.HandleSentMessageEventAsync(messageEvent);
        }

        public async Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId)
        {
            DateTime? createdSince = messagePage.Offset;
            Chat chat = await _chatRepository.GetChatMessagesWithParticipantsDetailsAsync(chatId, messagePage.Size, createdSince) 
                        ?? throw new Exception("Chat not found");
            List<ChatMessage> messages = chat.Messages;
            DateTime newOffset = messages.LastOrDefault()?.CreatedOn ?? createdSince ?? DateTime.Now;
            bool isLast = messages.Count != messagePage.Size;

            IEnumerable<MessageDTO> messageDTOs =
                messages.Select(
                    message => new MessageDTO()
                    {
                        Id = message.Id,
                        SentByFirstName = chat.Participants.Find(p => p.UserId == message.CreatedBy)!.User!.FirstName,
                        SentByLastName = chat.Participants.Find(p => p.UserId == message.CreatedBy)!.User!.LastName,
                        Content = message.Message,
                        Sent = message.CreatedOn
                    });

            return new()
            {
                Last = isLast,
                NextOffset = newOffset,
                Results = messageDTOs,
            };
        }
    }
}
