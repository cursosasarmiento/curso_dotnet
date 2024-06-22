using Curso.Domain.AspIdentity.DTOs.Requests;
using Curso.Domain.AspIdentity.DTOs.Responses;
using Microsoft.AspNetCore.Identity;

namespace Curso.Domain.AspIdentity.Contracts.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto dto);
        Task<RegisterResponseDto> Register(RegisterRequestDto dto);
        Task<IdentityResult> PatchUserDetails(string userId, UsuarioPatchRequestDto dto);
    }
}
