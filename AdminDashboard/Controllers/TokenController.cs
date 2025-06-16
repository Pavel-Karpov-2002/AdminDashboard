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
        public ActionResult Rate(string nameToken, float newRate)
        {
            var token = _tokenRepository.GetTokenByName(nameToken);
            if (token is null)
            {
                return BadRequest();
            }
            _tokenRepository.UpdateTokenRate(nameToken, newRate);
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<Token>> Rates()
        {
            List<Token> tokens = _tokenRepository.GetAll().ToList();
            return tokens;
        }
    }
}
