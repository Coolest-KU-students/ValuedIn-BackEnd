namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
