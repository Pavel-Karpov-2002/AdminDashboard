using AdminDashboard.DbStuff.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminDashboard.Services
{
    public class JwtProvider : IService
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login),
                new Claim("email", user.Email)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SECRET_KEY)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(JwtOptions.TIME_OF_LIFE_TOKEN));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            
            return tokenValue;
        }
    }
}
