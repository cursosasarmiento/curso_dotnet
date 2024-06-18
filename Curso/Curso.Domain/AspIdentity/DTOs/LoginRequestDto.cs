namespace Curso.Domain.AspIdentity.DTOs
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
