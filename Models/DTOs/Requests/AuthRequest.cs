namespace ValuedInBE.Models.DTOs.Requests
{
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
