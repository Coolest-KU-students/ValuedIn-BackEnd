using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class NewUser
    {
        [BindRequired]
        public string Login { get; set; } = null!;
        [BindRequired]
        public string Role { get; set; } = null!;
        [BindRequired]
        public string FirstName { get; set; } = string.Empty;
        [BindRequired]
        public string LastName { get; set; } = string.Empty;
        [BindRequired]
        public string Password { get; set; } = null!;
        [BindRequired]
        public string Email { get; set; } = string.Empty;
        [BindRequired]
        public string Telephone { get; set; } = string.Empty;
    }
}
