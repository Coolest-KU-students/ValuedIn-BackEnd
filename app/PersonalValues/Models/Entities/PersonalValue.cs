
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.PersonalValues.Models.Entities
{
    public class PersonalValue : AuditCreatedBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public long GroupId { get; set; }
        public short Modifier { get; set; }
        public PersonalValueGroup? Group { get; set; }
    }
}
