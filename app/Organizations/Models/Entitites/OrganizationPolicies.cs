using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Organizations.Models.Entitites
{
    public class OrganizationPolicies : AuditCreatedUpdateBase
    {
        [Key]
        public long OrganizationId { get; set; }
        public DateTime NextReevaluation { get; set; }
        public bool OpenPositions { get; set; }

        public Organization? Organization { get; set; }
    }
}
