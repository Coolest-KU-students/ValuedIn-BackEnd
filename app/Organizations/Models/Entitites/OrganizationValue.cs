
using System.ComponentModel.DataAnnotations;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Organizations.Models.Entitites
{
    public class OrganizationValue : AuditCreatedBase 
    {
        [Key]
        public long OrganizationId { get; set; }
        [Key]
        public long ValueId { get; set; }

        public Organization? Organization { get; set; }
        public PersonalValue? Value { get; set; }

    }
}
