using Newtonsoft.Json;
using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Responses.Authentication
{
    public class TokenAndRole
    {
        public string Token { get; init; }
        
        public string Role { get; init; }
    }
}
