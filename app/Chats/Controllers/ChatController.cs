﻿using Microsoft.AspNetCore.Mvc;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.DTOs.Response;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Chats.Services;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.System.WebConfigs.Middleware;

namespace ValuedInBE.Chats.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;
        private readonly IUserContextAccessor _userContextAccessor;

        public ChatController(IChatService chatService, ILogger<ChatController> logger, IUserContextAccessor userContextAccessor)
        {
            _chatService = chatService;
            _logger = logger;
            _userContextAccessor = userContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<OffsetPage<ChatInfo, DateTime>>> GetChatPagesAsync([FromQuery] ChatPageRequest chatPageRequest)
        {
            _logger.LogTrace("Received request from user {userId} to load {size} chats since {offset}", _userContextAccessor.UserContext.UserID, chatPageRequest.Size, chatPageRequest.Offset);
            return await _chatService.GetChatsAsync(chatPageRequest);
        }

        [HttpPost]
        public async Task<ActionResult<Chat?>> CreateNewChatAsync(NewChatRequest newChatRequest)
        {
            _logger.LogTrace("Received request from user {userId} to create new chat with these participants: {userIds}", _userContextAccessor.UserContext.UserID, string.Join(", ", newChatRequest.Participants));
            return await _chatService.FetchOrCreateChatAsync(newChatRequest);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<OffsetPage<MessageDTO, DateTime>>> GetMessagesAsync(long chatId, [FromQuery] MessagePageRequest messagePageRequest)
        {
            _logger.LogTrace("Received request from user {userId} to load {size} messages from chat {chatId} since {offset}", _userContextAccessor.UserContext.UserID, messagePageRequest.Size, chatId, messagePageRequest.Offset);
            return await _chatService.GetMessagesAsync(messagePageRequest, chatId);
        }

        [HttpPost("{chatId}")]
        public async Task<ActionResult<ChatMessage>> SendNewMessageAsync(long chatId, NewMessage newMessage)
        {
            _logger.LogTrace("Received request from user {userId} to create new message in chat {chatId}", _userContextAccessor.UserContext.UserID, chatId);
            return await _chatService.CreateNewMessageAsync(chatId, newMessage.Content);
        }
    }
}
