using Microsoft.Build.Framework;

namespace ValuedInBE.System.PersistenceLayer.Entities
{
    public class AuditCreatedBase : IAuditCreatedBase
    {
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string CreatedBy { get; set; } = null!;
    }
}
