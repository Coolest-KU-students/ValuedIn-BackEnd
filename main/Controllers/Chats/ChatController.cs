﻿using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.Services.Chats;
using ValuedInBE.System;
using ValuedInBE.System.Middleware;

namespace ValuedInBE.Controllers.Chats
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<OffsetPage<ChatInfo, DateTime>>> GetChatPages([FromQuery] ChatPageRequest chatPageRequest)
        {
            _logger.LogTrace("Received request from user {userId} to load {size} chats since {offset}", HttpContext.GetUserContext().UserID, chatPageRequest.Size, chatPageRequest.Offset);
            return await _chatService.GetChatsAsync(chatPageRequest);
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> CreateNewChat(NewChatRequest newChatRequest)
        {
            _logger.LogTrace("Received request from user {userId} to create new chat with these participants: {userIds}", HttpContext.GetUserContext().UserID, string.Join(", ", newChatRequest.Participants));
            return await _chatService.FetchOrCreateChatAsync(newChatRequest);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<OffsetPage<MessageDTO, DateTime>>> GetMessages(long chatId, [FromQuery] MessagePageRequest messagePageRequest)
        {
            _logger.LogTrace("Received request from user {userId} to load {size} messages from chat {chatId} since {offset}", HttpContext.GetUserContext().UserID, messagePageRequest.Size, chatId, messagePageRequest.Offset);
            return await _chatService.GetMessagesAsync(messagePageRequest, chatId);
        }

        [HttpPost("{chatId}")]
        public async Task<ActionResult<ChatMessage>> SendNewMessage(long chatId, NewMessage newMessage)
        {
            _logger.LogTrace("Received request from user {userId} to create new message in chat {chatId}", HttpContext.GetUserContext().UserID, chatId);
            return await _chatService.CreateNewMessageAsync(chatId, newMessage.Content);
        }
    }
}
