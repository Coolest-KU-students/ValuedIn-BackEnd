using Microsoft.Build.Framework;

namespace ValuedInBE.Tokens.Models
{
    public class TokenData<T>
    {
        [Required]
        public T Value { get; set; } = default!;
        [Required]
        public string Type { get; set; } = string.Empty;
    }
}
