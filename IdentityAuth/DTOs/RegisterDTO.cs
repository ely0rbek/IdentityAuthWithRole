namespace IdentityAuth.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public int Age { get; set; }
        public List<string> Roles { get; set; }
    }
}
