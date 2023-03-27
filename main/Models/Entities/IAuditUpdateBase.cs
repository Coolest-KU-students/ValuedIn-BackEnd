namespace ValuedInBE.Models.Entities
{
    public interface IAuditCreateUpdateBase : IAuditCreatedBase
    {
        DateTime UpdatedOn { get; set; }
        string UpdatedBy { get; set; }
    }
}
