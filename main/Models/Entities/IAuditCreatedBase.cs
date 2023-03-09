namespace ValuedInBE.Models.Entities
{
    public interface IAuditCreatedBase
    {
        DateTimeOffset CreatedOn { get; set; }
        string CreatedBy { get; set; }
    }
}
