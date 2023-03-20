namespace ValuedInBE.System.PersistenceLayer.Entities
{
    public class AuditCreatedUpdateBase : AuditCreatedBase, IAuditCreateUpdateBase
    {
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
