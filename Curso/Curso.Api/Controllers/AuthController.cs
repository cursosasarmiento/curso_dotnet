﻿using Curso.Domain.AspIdentity.Contracts.Services;
using Curso.Domain.AspIdentity.DTOs.Requests;
using Curso.Domain.AspIdentity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Curso.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) && string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Debes enviar un Username o un Email");
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("La contraseña es obligatoria");

            var response = await _authService.Login(dto);
            return Ok(response);
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequestDto dto)
        {
            var roleString = dto.Role.ToString();
            if (!roleString.Equals(RoleEnum.Arrendatario.ToString()))
            {
                return BadRequest("El role no es valido");
            }

            if(string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Username))
            {
                return BadRequest("El username y el email son obligatorios");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("La contraseña es obligatoria");
            if (string.IsNullOrWhiteSpace(dto.PasswordRetyped))
                return BadRequest("La comprobacion de la contraseña es obligatoria");
            if (!dto.Password.Equals(dto.PasswordRetyped))
                return BadRequest("Las contraseñas deben ser iguales");

            var response = await _authService.Register(dto);
            return Ok(response);
        }
    }
}
