namespace ValuedInBE.System.PersistenceLayer.Entities
{
    public class AuditCreatedBase : IAuditCreatedBase
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
