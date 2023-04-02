namespace ValuedInBE.Users.Models.DTOs.Request
{
    public class NewUser
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
