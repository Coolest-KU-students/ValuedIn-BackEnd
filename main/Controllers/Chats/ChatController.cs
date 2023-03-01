using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
using ValuedInBE.Services.Chats;
using ValuedInBE.System;

namespace ValuedInBE.Controllers.Chats
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {/*
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("page")]
        public async Task<ActionResult<Page<ChatLatestInfo>>> GetChatPages(ChatPageRequest chatPageRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.GetLatestChats(chatPageRequest, userContext);
        }

        [HttpPost("new")]
        public async Task<ActionResult<ChatLatestInfo>> CreateNewChat(NewChatRequest newChatRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.FetchOrCreateAChat(newChatRequest, userContext);
        }

        [HttpPost("{chatId}")]
        public async Task<ActionResult<List<MessageDTO>>> GetMessages(long chatId, MessagePageRequest messagePageRequest)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.GetMessages(messagePageRequest, chatId, userContext);
        }

        [HttpPost("{chatId}/new")]
        public async Task<ActionResult<MessageDTO>> SendNewMessage(long chatId, NewMessage newMessage)
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.CreateNewMessage(chatId, newMessage, userContext);
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<List<ChatLatestInfo>>> CheckInForAnythingNew()
        {
            UserContext userContext = HttpContext.GetUserContext();
            return await _chatService.CheckIn();
        }
        */
    }
}
