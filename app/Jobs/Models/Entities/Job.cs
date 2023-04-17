using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.Organizations.Models.Entitites;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Jobs.Models.Entities
{
    public class Job : AuditCreatedUpdateBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }   
        public long OrganizationId { get; set; }
        [Required]
        public string? Title { get; set; }
        public byte[]? Banner { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateOnly ValidUntil { get; set; }

        public Organization? Organization { get; set; }
        public List<JobValue>? JobValues { get; set; }
        public List<JobApplicants>? JobApplicants { get; set;}
    }
}
