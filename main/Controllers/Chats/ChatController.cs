using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
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

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<ActionResult<OffsetPage<ChatInfo, DateTime>>> GetChatPages([FromQuery] ChatPageRequest chatPageRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.GetChatsAsync(chatPageRequest, userContext);
        }

        [HttpPost]
        public async Task<ActionResult<ChatInfo>> CreateNewChat(NewChatRequest newChatRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.FetchOrCreateChatAsync(newChatRequest, userContext);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<OffsetPage<MessageDTO, DateTime>>> GetMessages(long chatId, [FromQuery] MessagePageRequest messagePageRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.GetMessagesAsync(messagePageRequest, chatId, userContext);
        }

        [HttpPost("{chatId}")]
        public async Task<ActionResult<MessageDTO>> SendNewMessage(long chatId, NewMessage newMessage)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.CreateNewMessageAsync(chatId, newMessage, userContext);
        }

    }
}
