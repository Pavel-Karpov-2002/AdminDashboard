using AdminDashboard.DbStuff.Models;
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
        private readonly JwtProvider _jwtProvider;

        public AuthController(UserRepository userRepository, AuthService authService, JwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _authService = authService;
            _jwtProvider = jwtProvider;
        }

        [HttpPost("registration")]
        public ActionResult Registration([Bind("Login,Password,Email")] User user)
        {
            try
            {
                _userRepository.Add(user);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
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
            var user = _userRepository.GetUserByEmail(email);
            if (user is null)
            {
                return BadRequest();
            }

            var isAuthorize = _authService.PasswordVerify(password, user.Password);

            if (isAuthorize == false)
            {
                return BadRequest();
            }

            var token = _jwtProvider.GenerateToken(user);
            _authService.SignInUser(token);
            return Ok(token);
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
