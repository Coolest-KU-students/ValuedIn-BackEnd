﻿
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using ValuedInBETests.TestingConstants;
using Microsoft.AspNetCore.Http;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Chats.Controllers;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.DTOs.Response;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Chats.Services;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.TestingConstants;
using ValuedInBE.System.UserContexts.Accessors;

namespace ValuedInBE.Controllers.Chats.Tests
{

    public class ChatControllerTests
    {
        private readonly Mock<ILogger<ChatController>> _logger = new();
        private readonly Mock<IChatService> _chatService = new();
        private readonly Mock<IUserContextAccessor> _mockUserContextAccessor = new();


        private ChatController MockChatController()
        {
            _mockUserContextAccessor.SetupGet(accessor => accessor.UserContext)
                .Returns(UserConstants.UserContextInstance);
            ChatController controller = new(_chatService.Object, _logger.Object, _mockUserContextAccessor.Object);
            return controller;
        }


        [Fact()]
        public async Task TestGetChatPages()
        {
            ChatPageRequest chatPageRequest = new()
            {
                Offset = DateTime.Now,
                Size = 25
            };
            OffsetPage<ChatInfo, DateTime> offsetPage = new()
            {
                Last = true,
                NextOffset = DateTime.Now,
                Results = new List<ChatInfo>() { }
            };
            _chatService.Setup(service => service.GetChatsAsync(chatPageRequest))
                .ReturnsAsync(offsetPage);
            
            ChatController controller = MockChatController();
            ActionResult<OffsetPage<ChatInfo, DateTime>> getPagesActionResult = await controller.GetChatPagesAsync(chatPageRequest);

            Assert.NotNull(getPagesActionResult.Value);
            OffsetPage<ChatInfo, DateTime> receivedOffsetPage = getPagesActionResult.Value!;
            Assert.Equal(offsetPage, receivedOffsetPage);
        }

        [Fact()]
        public async Task TestCreateNewChatAsync()
        {
            NewChatRequest newChatRequest = new()
            {
                MessageContent = "This is a message",
                Participants = new() { "participant1" }
            };
            Chat chat = new()
            {
                Id = 1,
                Messages = new() { ChatConstants.ChatMessage },
                Participants = new(),
                CreatedBy = "user",
                CreatedOn = DateTime.Now,
            };

            _chatService.Setup(service => service.FetchOrCreateChatAsync(newChatRequest))
                .ReturnsAsync(chat);
            ChatController controller = MockChatController();
            ActionResult<Chat?> receivedActionResult = await controller.CreateNewChatAsync(newChatRequest);
            Assert.NotNull(receivedActionResult.Value);
            Chat receivedChat = receivedActionResult.Value!;
            Assert.Equal(chat, receivedChat);
        }

        [Fact()]
        public async Task TestGetMessagesAsync()
        {
            long chatId = ChatConstants.chatID;
            MessagePageRequest messagePageRequest = new()
            {
                Offset = DateTime.Now,
                Size = 25
            };
            OffsetPage<MessageDTO, DateTime> offsetPage = new() { 
                Last = true,
                NextOffset = DateTime.Now,
                Results = new List<MessageDTO>()
            };
            _chatService.Setup(service => service.GetMessagesAsync(messagePageRequest, chatId))
                .ReturnsAsync(offsetPage);

            ChatController controller = MockChatController();
            ActionResult<OffsetPage<MessageDTO, DateTime>> actionResult = await controller.GetMessagesAsync(chatId, messagePageRequest);
            Assert.NotNull(actionResult.Value);
            OffsetPage<MessageDTO, DateTime> receivedOffsetPage = actionResult.Value!;
            Assert.Equal(offsetPage, receivedOffsetPage);
        }

        [Fact()]
        public async Task TestSendNewMessage()
        {
            long chatId = ChatConstants.chatID;
            NewMessage newMessage = new()
            {
                Content = "this is new message"
            };
            ChatMessage chatMessage = new()
            {
                ChatId = chatId,
                Id = 1,
                Message = "this is new message",
                CreatedBy = "user",
                CreatedOn = DateTime.Now,
            };
            _chatService.Setup(service => service.CreateNewMessageAsync(chatId, "this is new message"))
                .ReturnsAsync(chatMessage);

            ChatController controller = MockChatController();
            ActionResult<ChatMessage> actionResult = await controller.SendNewMessageAsync(chatId, newMessage);
            Assert.NotNull(actionResult.Value);
            ChatMessage receivedChatMessage = actionResult.Value!;
            Assert.Equal(chatMessage, receivedChatMessage);
        }
    }
}