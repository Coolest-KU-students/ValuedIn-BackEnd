
using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models.Entities
{
    public class UserCredentials : AuditCreatedUpdateBase
    {
        [MaxLength(128)]
        public string Login { get; set; }
        [Key]
        [MaxLength(128)]
        public string UserID { get; set; }
        public string Password { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        public UserRole Role { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
