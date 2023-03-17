﻿using ValuedInBE.DataControls.Memory;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Events.Handlers;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.Models.Events;
using ValuedInBE.Repositories;
using ValuedInBE.Services.Users;
using ValuedInBE.System.Middleware;

namespace ValuedInBE.Services.Chats.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly ILogger<ChatService> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly MessageEventHandler _eventHandler;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger, IHttpContextAccessor contextAccessor, MessageEventHandler eventHandler, IUserService userService)
        {
            _chatRepository = chatRepository;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _eventHandler = eventHandler;
            _userService = userService;
        }

        public async Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage)
        {
            DateTime? createdSince = chatPage.Offset;
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
            IEnumerable<Chat> chats = await _chatRepository.GetChatsWithLastMessageAndParticipantsAsync(userContext.UserID, chatPage.Size, chatPage.Offset);
            DateTime newOffset = chats.LastOrDefault()?.CreatedOn ?? createdSince ?? DateTime.Now;
            bool isLast = !chats.Any();

            IEnumerable<ChatInfo> chatInfos =
                chats.Select(
                chat => new ChatInfo()
                {
                    Id = chat.Id,
                    ChatName = "Lorem Ipsum",
                    LastMessage = chat.Messages.First().CreatedOn,
                    LastMessageContent = chat.Messages.First()?.Message,
                    ParticipatingUsers = chat.Participants.Select(p => p.UserId).ToList(),
                    Unread = true
                });

            return new()
            {
                Last = chats.Any(),
                NextOffset = newOffset,
                Results = chatInfos,
            };
        }

        public async Task<Chat> FetchOrCreateChatAsync(NewChatRequest newChatRequest)
        {
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
            if (newChatRequest.Participants.Count == (newChatRequest.Participants.Contains(userContext.UserID) ? 1 : 0))
                throw new Exception("No other participants but you");

            Task<bool> usersExistTask = _userService.VerifyUserIdExistenceAsync(newChatRequest.Participants);

            if (!await usersExistTask)
            {
                throw new Exception("Not all participants exist");
            }

            List<string> allParticipants = newChatRequest.Participants;
            allParticipants.Add(userContext.UserID);

            Chat existingChat = await _chatRepository.GetChatFromParticipantsAsync(allParticipants);

            if (existingChat != null) return existingChat;

            Chat newChat = new();
            await _chatRepository.SaveNewChatAsync(newChat);
            long newChatId = newChat.Id;

            List<ChatParticipant> chatParticipants =
                allParticipants.Select(
                    p => new ChatParticipant() {
                        ChatId = newChatId,
                        UserId = p
                    }
                ).ToList();

            await _chatRepository.AddChatParticipantsAsync(chatParticipants);

            ChatMessage chatMessage = await CreateNewMessageAsync(newChatId, newChatRequest.MessageContent);

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

        public async Task SendMessageEvent(ChatMessage chatMessage, List<string> otherParticipants = null)
        {
            otherParticipants ??= await _chatRepository.GetParticipantIdsFromChat(chatMessage.ChatId); 
            NewMessageEvent messageEvent = new()
            {
                ChatMessage = chatMessage,
                OtherParticipantIDs = otherParticipants,
            };

            await _eventHandler.HandleSentMessageEvent(messageEvent);
        }

        public async Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId)
        {
            DateTime? createdSince = messagePage.Offset;
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
            Chat chat = await _chatRepository.GetChatMessagesWithParticipantsDetails(chatId, messagePage.Size, createdSince);
            if(chat == null)
            {
                throw new Exception("Chat not found");
            }
            
            List<ChatMessage> messages = chat.Messages;
            DateTime newOffset = messages.LastOrDefault()?.CreatedOn ?? createdSince ?? DateTime.Now;
            bool isLast = !messages.Any();

            IEnumerable<MessageDTO> messageDTOs =
                messages.Select(
                    message => new MessageDTO()
                    {
                        Id = message.Id,
                        SentByFirstName = chat.Participants.Find(p => p.UserId == message.CreatedBy).User.FirstName,
                        SentByLastName = chat.Participants.Find(p => p.UserId == message.CreatedBy).User.LastName,
                        Content = message.Message,
                        Sent = message.CreatedOn
                    });

            return new()
            {
                Last = messages.Any(),
                NextOffset = newOffset,
                Results = messageDTOs,
            };
        }
    }
}
