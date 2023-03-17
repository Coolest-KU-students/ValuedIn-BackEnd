namespace ValuedInBE.Models.Entities
{
    public class AuditCreatedUpdateBase : AuditCreatedBase, IAuditCreateUpdateBase
    {
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
