using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.DbStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(SocialNetworkWebDbContext context) : base(context) { }

        public User GetUserWithPaymentsAndTokenBalance(int id)
        => _entyties
            .Include(user => user.Payments)
            .Include(user => user.TokenBalance)
            .FirstOrDefault(user => user.Id == id);

        public List<User> GetAllUsersWithPaymentsAndTokenBalance()
        => _entyties
            .Include(user => user.Payments)
            .Include(user => user.TokenBalance)
            .ToList();

        public User GetUserWithPayments(int id)
        => _entyties
            .Include(user => user.Payments)
            .FirstOrDefault(user => user.Id == id);

        public User? GetUserByEmail(string email)
            => _entyties
            .FirstOrDefault(user => user.Email.Equals(email));
    }
}
