using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("rate")]
    public class TokenController : ControllerBase
    {
        private readonly TokenRepository _tokenRepository;

        public TokenController(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        [HttpGet]
        public ActionResult<Token> Rate(string nameToken)
        {
            return _tokenRepository.GetTokenByName(nameToken);
        }

        [HttpPost]
        public ActionResult<bool> Rate(string nameToken, float newRate)
        {
            var token = _tokenRepository.GetTokenByName(nameToken);
            if (token is null)
            {
                return BadRequest();
            }
            _tokenRepository.UpdateTokenRate(nameToken, newRate);
            return Ok();
        }
    }
}
