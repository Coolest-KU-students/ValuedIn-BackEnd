using System.ComponentModel.DataAnnotations;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Users.Models.Entities
{
    public class UserValue : AuditCreatedBase
    {
        [Required]
        [MaxLength(128)]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public long ValueId { get; set; }

        public UserDetails? UserDetails { get; set; }
        public PersonalValue? Value { get; set; }
    }
}
