using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.PersonalValues.Models.DTOs.Requests
{
    public class NewValue
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public long GroupId { get; set; }
        public short? Modifier { get; set; }
    }
}
