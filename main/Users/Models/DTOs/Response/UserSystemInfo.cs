using Microsoft.Build.Framework;

namespace ValuedInBE.Users.Models.DTOs.Response
{
    public class UserSystemInfo
    {
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string UserID { get; set; } = null!;
        [Required]
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        [Required] 
        public string Role { get; set; } = null!; 
        [Required] 
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Telephone { get; set; } = string.Empty;
    }
}