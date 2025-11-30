using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace XRayJoural.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> GetToken(string login)
        {
            var claims = CreateClaims(login);

            var token = new JwtSecurityToken(
                issuer: "MyAuth",
                notBefore: DateTime.Now,
                claims: claims.Claims,
                expires: DateTime.Now.AddMinutes(120),
                audience: "Potreb",
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256)
                );

            var result = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(result);
        }

        private ClaimsIdentity CreateClaims(string login)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim("id", "123")
            };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
