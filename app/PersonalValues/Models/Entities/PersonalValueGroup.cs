using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.PersonalValues.Models.Entities
{
    public class PersonalValueGroup : AuditCreatedBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty; 
    
        public List<PersonalValue>? PersonalValues { get; set; }
    }
}
