using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.DbStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(SocialNetworkWebDbContext context) : base(context)
        {
        }

        public async Task<User>? GetUserWithPaymentsAndTokenBalance(int id)
        => await _entyties
            .Include(user => user.Payments)
            .Include(user => user.TokenBalance)
            .FirstOrDefaultAsync(user => user.Id == id);
    }
}
