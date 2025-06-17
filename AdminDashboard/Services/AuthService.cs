using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services.Interfaces;

namespace AdminDashboard.Services
{
    public class AuthService : IService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool PasswordVerify(string password, string passwordVerify)
        {
            bool isVerifed = passwordVerify.Equals(password);
            return isVerifed;
        }

        public void SignInUser(string token)
        {
            _httpContextAccessor
                .HttpContext.Response.Cookies.Append("JWT", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
        }
    }
}