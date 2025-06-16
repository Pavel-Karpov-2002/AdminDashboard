using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("rate")]
    public class TokenController
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
    }
}
