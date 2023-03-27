
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Users.Models.DTOs.Response
{
    public class TokenAndRole
    {
        [Required]
        public string Token { get; init; } = null!;
        [Required]
        public string Role { get; init; } = null!;
    }
}
