using Curso.Domain.AspIdentity.DTOs;

namespace Curso.Domain.AspIdentity.Contracts.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto dto);
    }
}
