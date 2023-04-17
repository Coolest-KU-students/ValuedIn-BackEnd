using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Organizations.Models.Entitites
{
    [Index(nameof(Name), IsUnique=true)]
    public class Organization : AuditCreatedUpdateBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [StringLength(250, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;
    
        public OrganizationDetails? Details { get; set; }
        public OrganizationPolicies? Policies { get; set; }
        public List<OrganizationValue>? Values { get; set; } 
    }
}
