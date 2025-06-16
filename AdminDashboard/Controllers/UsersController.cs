using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("clients")]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = _userRepository.GetAll().ToList();
            return users;
        }

        [HttpGet]
        public ActionResult<User> User(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return BadRequest();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create([Bind("Login,Password,Email,Id")] User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Add(user);
                return CreatedAtAction(nameof(User), new { id = user.Id });
            }
            return BadRequest();
        }
    }
}
