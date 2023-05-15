using ValuedInBE.PersonalValues.Models.DTOs.Pocos;

namespace ValuedInBE.PersonalValues.Models.DTOs.Response
{
    public class ValuesInGroup
    {
        public string GroupName { get; set; } = string.Empty;
        public IEnumerable<IdAndValue> Values { get; set; } = Enumerable.Empty<IdAndValue>();
    }
}
