using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AdminDashboard.Services
{
    public class AuthService : IService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public const string AUTH_KEY = "keyYEKkey";

        public AuthService(UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login ?? "user"),
                new Claim("email", user.Email ?? "")
            };

            var identity = new ClaimsIdentity(claims, AUTH_KEY);
            var principal = new ClaimsPrincipal(identity);
            _httpContextAccessor
                .HttpContext
                .SignInAsync(AUTH_KEY, principal)
                .Wait();
        }
    }
}