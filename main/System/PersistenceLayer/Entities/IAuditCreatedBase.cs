namespace ValuedInBE.System.PersistenceLayer.Entities
{
    public interface IAuditCreatedBase
    {
        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }
    }
}
