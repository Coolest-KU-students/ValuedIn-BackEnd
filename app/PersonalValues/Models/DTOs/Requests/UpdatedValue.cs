using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.PersonalValues.Models.DTOs.Requests
{
    public class UpdatedValue
    {
        [Required]
        public long ValueId { get; set; }
        public string? Name { get; set; }
        public short? Modifier { get; set; }
        public long? GroupId { get; set; }
    }
}
