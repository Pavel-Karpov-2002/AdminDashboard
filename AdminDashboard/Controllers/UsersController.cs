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

        [HttpGet("clients")]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = _userRepository.GetAllUsersWithPaymentsAndTokenBalance();
            return users;
        }

        [HttpGet]
        public ActionResult<User> User(int id)
        {
            var user = _userRepository.GetUserWithPaymentsAndTokenBalance(id);

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

        [HttpGet("payments")]
        public ActionResult<List<Payment>> GetPayments(int userId, int take)
        {
            User user = _userRepository.GetUserWithPayments(userId);

            if (user is null)
            {
                return BadRequest();
            }

            int lastPayment = user.Payments.Count - take;

            if (lastPayment < 0)
            {
                return BadRequest();
            }

            List<Payment> payments = user.Payments.GetRange(lastPayment, take);
            return payments;
        }

        [HttpDelete("delete")]
        public ActionResult Delete(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("update")]
        public ActionResult Update(int id, [FromBody]User editedUser)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                return BadRequest();
            }

            user.Login = editedUser.Login;
            user.Password = editedUser.Password;
            user.Email = editedUser.Email;

            if (editedUser.TokenBalance is not null)
            {
                user.TokenBalance = editedUser.TokenBalance;
            }
            _userRepository.Update(user);
            return Ok();
        }
    }
}
