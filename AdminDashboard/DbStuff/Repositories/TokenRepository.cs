using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.DbStuff.Repositories
{
    public class TokenRepository : BaseRepository<Token>
    {
        public TokenRepository(SocialNetworkWebDbContext context) : base(context)
        {
        }
    }
}
