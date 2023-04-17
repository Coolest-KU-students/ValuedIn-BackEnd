
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class AuthRequest
    {
        [BindRequired]
        public string Login { get; set; } = null!;

        [BindRequired]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
