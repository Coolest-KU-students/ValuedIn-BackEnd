using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class UpdatedUser
    {
        [BindRequired]
        public string UserID { get; set; } = null!;
        [BindRequired]
        public string FirstName { get; set; } = string.Empty;
        [BindRequired]
        public string LastName { get; set; } = string.Empty;
        [BindRequired]
        public string Role { get; set; } = null!;
        [BindRequired]
        public string Email { get; set; } = string.Empty;
        [BindRequired]
        public string Telephone { get; set; } = string.Empty;
    }
}
