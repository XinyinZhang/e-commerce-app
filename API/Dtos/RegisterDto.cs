namespace API.Dtos
{
    // contains all information we need to register a new user
    public class RegisterDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}