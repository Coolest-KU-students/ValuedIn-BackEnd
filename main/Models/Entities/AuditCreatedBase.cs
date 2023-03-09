namespace ValuedInBE.Models.Entities
{
    public class AuditCreatedBase : IAuditCreatedBase
    {
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
