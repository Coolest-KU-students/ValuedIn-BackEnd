namespace ValuedInBE.Users.Models.DTOs.Response
{
    public class UserSystemInfo
    {
        public string Login { get; set; }
        public string UserID { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}