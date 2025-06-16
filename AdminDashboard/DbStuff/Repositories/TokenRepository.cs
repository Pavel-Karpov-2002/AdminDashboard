using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.DbStuff.Repositories
{
    public class TokenRepository : BaseRepository<Token>
    {
        public TokenRepository(SocialNetworkWebDbContext context) : base(context) { }

        public Token GetTokenByName(string nameToken) =>
        _entyties
        .FirstOrDefault(token => token.NameToken == nameToken);

        public Token UpdateTokenRate(string nameToken, float rate)
        {
            var token = _entyties.FirstOrDefault(token => token.NameToken == nameToken);
            token.Rate = rate;
            _context.SaveChanges();
            return token;
        }
    }
}
