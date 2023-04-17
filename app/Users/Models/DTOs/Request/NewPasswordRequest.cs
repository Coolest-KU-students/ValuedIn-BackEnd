using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class NewPasswordRequest
    {
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        public string OldPassword { get; set; } = string.Empty;
    }
}
