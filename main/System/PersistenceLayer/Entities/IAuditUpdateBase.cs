namespace ValuedInBE.System.PersistenceLayer.Entities
{
    public interface IAuditCreateUpdateBase : IAuditCreatedBase
    {
        DateTime? UpdatedOn { get; set; }
        string? UpdatedBy { get; set; }
    }
}
