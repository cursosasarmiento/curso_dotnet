using Curso.AspIdentity.Entities;
using Curso.Domain.AspIdentity;
using Curso.Domain.AspIdentity.Contracts.Services;
using Curso.Domain.AspIdentity.DTOs.Requests;
using Curso.Domain.AspIdentity.DTOs.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Curso.Application.AspIdentity.Services
{
    public class AuthService(UserManager<UsuarioIdentity> userManager, SignInManager<UsuarioIdentity> signInManager, JwtSettings jwtSettings) : IAuthService
    {
        private readonly UserManager<UsuarioIdentity> _userManager = userManager;
        private readonly SignInManager<UsuarioIdentity> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = jwtSettings;

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            UsuarioIdentity user;
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    throw new Exception($"El email: {dto.Email} no puede ser encontrado");
            }
            else if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                    throw new Exception($"El username: {dto.Username} no puede ser encontrado");
            }
            else
            {
                throw new Exception("No se enviaron ni email ni username");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("El password es obligatorio");
            SignInResult login = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!login.Succeeded)
                throw new Exception("Usuario o contraseña no validos");
            JwtSecurityToken token = await GenerateToken(user);

            LoginResponseDto response = new LoginResponseDto()
            {
                UserId = user.Id,
                Nombre = user.Nombre,
                Apellidos = user.Apellidos,
                Email = user.Email,
                Username = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return response;
        }

        
        public async Task<RegisterResponseDto> Register(RegisterRequestDto dto)
        {
            var user = new UsuarioIdentity()
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = false
            };

            if (!dto.Password.Equals(dto.PasswordRetyped))
                throw new Exception("Las contraseñas no son iguales");

            var result = await _userManager.CreateAsync(user, dto.Password);
            if(!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(x => x.Description));
                throw new Exception($"Hubo errores durante la creacion del usuario: {errors}");
            }
            var roleCheck = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            if (!roleCheck.Succeeded)
            {
                var errors = string.Join("; ", roleCheck.Errors.Select(x => x.Description));
                throw new Exception($"Hubo errores durante la adicion de roles al usuario: {errors}");
            }

            var token = await GenerateToken(user);

            RegisterResponseDto response = new RegisterResponseDto()
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            return response;
        }

        #region Private Methods
        private async Task<JwtSecurityToken> GenerateToken(UsuarioIdentity user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(roles => new Claim(ClaimTypes.Role, roles));
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claimList = new List<Claim>()
            {
                new Claim("Uid", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            claimList.AddRange(roleClaims);
            claimList.AddRange(userClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claimList,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Time),
                signingCredentials: credentials
                );
            return token;
        }

        #endregion Privete Methods

    }
}
