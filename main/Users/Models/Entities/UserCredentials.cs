
using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models.Entities
{
    public class UserCredentials : AuditCreatedUpdateBase
    {
        [Key]
        [MaxLength(128)]
        [Required]
        public string UserID { get; set; } = null!;
        [MaxLength(128)]
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public UserDetails? UserDetails { get; set; }
    }
}
