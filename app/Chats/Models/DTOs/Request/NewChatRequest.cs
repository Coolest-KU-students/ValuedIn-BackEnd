using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Chats.Models.DTOs.Request
{
    public class NewChatRequest
    {
        [BindRequired]
        public List<string> Participants { get; set; } = null!;

        [BindRequired]
        public string MessageContent { get; set; } = null!;
    }
}