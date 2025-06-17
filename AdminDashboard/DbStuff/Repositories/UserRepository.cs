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
            .ThenInclude(tokenBalance => tokenBalance.Token)
            .FirstOrDefault(user => user.Id == id);

        public List<User> GetAllUsersWithPaymentsAndTokenBalance()
        => _entyties
            .Include(user => user.Payments)
            .Include(user => user.TokenBalance)
            .ThenInclude(tokenBalance => tokenBalance.Token)
            .ToList();

        public User GetUserWithPayments(int id)
        => _entyties
            .Include(user => user.Payments)
            .FirstOrDefault(user => user.Id == id);

        public User? GetUserByEmail(string email)
            => _entyties
            .FirstOrDefault(user => user.Email.Equals(email));

        public User UpdateUser(User editedUser)
        {
            var user = GetUserWithPaymentsAndTokenBalance(editedUser.Id);
            user.Email = editedUser.Email;
            user.Login = editedUser.Login;
            user.Password = editedUser.Password;

            foreach (var paymentUser in editedUser.Payments)
            {
                var payment = user.Payments.FirstOrDefault(p => p.Id == paymentUser.Id);
                if (payment != null)
                {
                    _context.Entry(payment).CurrentValues.SetValues(paymentUser);
                }
                else
                {
                    user.Payments.Add(paymentUser);
                }
            }

            foreach (var tokenUser in editedUser.TokenBalance)
            {
                var token = user.TokenBalance.FirstOrDefault(t => t.Id == tokenUser.Id);
                if (token != null)
                {
                    _context.Entry(token).CurrentValues.SetValues(tokenUser);
                }
                else
                {
                    user.TokenBalance.Add(tokenUser);
                }
            }
            _context.SaveChanges();
            return user;
        }
    }
}
