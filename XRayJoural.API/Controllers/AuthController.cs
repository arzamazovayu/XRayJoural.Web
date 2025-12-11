using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using XRayJournal.BLL;
using XRayJournal.Core;
using XRayJournal.Core.Models;
using XRayJournal.Core.Results;

namespace XRayJoural.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        ErrorMessage = "Логин и пароль обязательны"
                    });
                }

                // Проверяем логин/пароль в БД
                var authResult = await _userService.AuthenticateAsync(request.Login, request.Password);

                if (!authResult.Success)
                {
                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        ErrorMessage = authResult.ErrorMessage
                    });
                }

                var user = authResult.Data;

                // Создаем токен
                var token = GenerateJwtToken(user.Login, user.Role);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка сервера: {ex.Message}"
                });
            }
        }

        private string GenerateJwtToken(string login, UserRole role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("RoleId", ((int)role).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MyAuth",
                audience: "Potreb",
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
