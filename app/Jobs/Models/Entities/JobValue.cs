using System.ComponentModel.DataAnnotations;
using ValuedInBE.Organizations.Models.Entitites;
using ValuedInBE.PersonalValues.Models.Entities;

namespace ValuedInBE.Jobs.Models.Entities
{
    public class JobValue
    {
        [Key]
        public long JobId { get; set; }
        [Key]
        public long ValueId { get; set; }

        public Job? Job { get; set; }
        public PersonalValue? Value { get; set; }
    }
}
