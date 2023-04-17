using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Organizations.Models.Entitites
{
    public class OrganizationDetails : AuditCreatedUpdateBase
    {
        [Key]
        public long OrganizationId { get; set; }

        public string? Description { get; set; }

        public Organization? Organization { get; set; }
    }
}
