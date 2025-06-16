using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(UserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<bool> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(true);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<bool> Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(email, password);
            if (user is null)
            {
                return BadRequest();
            }

            _authService.SignInUser(user);
            return Ok(true);
        }

        [HttpPost]
        [Route("logout")]
        public ActionResult<bool> Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Ok(true);
        }
    }
}
