namespace ValuedInBE.Models.Entities
{
    public interface IAuditCreatedBase
    {
        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }
    }
}
