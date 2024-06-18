using Curso.AspIdentity.Entities;
using Curso.Domain.AspIdentity;
using Curso.Domain.AspIdentity.Contracts.Services;
using Curso.Domain.AspIdentity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
            }else if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                user = await _userManager.FindByNameAsync(dto.Username);
                if(user == null)
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
    }
}
