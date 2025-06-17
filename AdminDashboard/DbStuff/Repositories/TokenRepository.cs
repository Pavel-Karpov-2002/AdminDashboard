using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.DbStuff.Repositories
{
    public class TokenRepository : BaseRepository<Token>
    {
        public TokenRepository(SocialNetworkWebDbContext context) : base(context) { }

        public Token GetTokenByName(string nameToken) =>
        _entyties
        .FirstOrDefault(token => token.NameToken == nameToken);

        public Token UpdateTokenRate(int id, float rate)
        {
            var token = GetById(id);
            token.Rate = rate;
            _context.SaveChanges();
            return token;
        }

        public Token UpdateTokenName(int id, string nameToken)
        {
            var token = GetById(id);
            token.NameToken = nameToken;
            _context.SaveChanges();
            return token;
        }
    }
}
