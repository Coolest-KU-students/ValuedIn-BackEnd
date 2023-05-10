using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Organizations.Models.DTOs.Requests
{
    public class NewOrganization
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Telephone { get; set; } = string.Empty;

    }
}
