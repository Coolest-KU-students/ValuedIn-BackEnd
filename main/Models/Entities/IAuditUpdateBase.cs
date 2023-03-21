namespace ValuedInBE.Models.Entities
{
    public interface IAuditCreateUpdateBase : IAuditCreatedBase
    {
        DateTimeOffset UpdatedOn { get; set; }
        string UpdatedBy { get; set; }
    }
}
