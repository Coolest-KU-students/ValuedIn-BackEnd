using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Jobs.Models.Entities
{
    public class JobApplicants : AuditCreatedUpdateBase
    {
        [Key]
        public long JobId { get; set; }
        [Key]
        public long UserId { get; set; }
        public byte[]? CV { get; set; }
        public bool Viewed { get; set; }

        public Job? Job { get; set; }
        public UserDetails? Applicant { get; set; }
    }
}
