using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.DbStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(SocialNetworkWebDbContext context) : base(context)
        {
        }
    }
}
