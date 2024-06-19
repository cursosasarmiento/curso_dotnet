using Curso.Domain.AspIdentity.Enums;

namespace Curso.Domain.AspIdentity.DTOs.Requests
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordRetyped { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public RoleEnum Role { get; set; } = RoleEnum.Arrendatario;
        
    }
}
