namespace ValuedInBE.Models.Entities
{
    public class AuditCreatedUpdateBase : AuditCreatedBase, IAuditCreateUpdateBase
    {
        public DateTimeOffset UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
