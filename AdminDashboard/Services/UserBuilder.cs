using AdminDashboard.DbStuff.Models;
using AdminDashboard.Services.Interfaces;

namespace AdminDashboard.Services
{
    public class UserBuilder : IService
    {
        public User BuildUser(string login, string email, string password, List<UserToken> tokenBalance, List<Payment> payments)
        {
            return new User()
            {
                Login = login,
                Password = password,
                Email = email,
                TokenBalance = tokenBalance,
                Payments = payments
            };
        }

        public UserToken BuildUserToken(Token token, float countToken)
        {
            return new UserToken()
            {
                Token = token,
                CountToken = countToken
            };
        }

        public Payment BuildPayment(string paymentName, float paymentCost, DateTime dateOfPurchase)
        {
            return new Payment()
            {
                DateOfPurchase = dateOfPurchase,
                PaymentName = paymentName,
                PaymentCost = paymentCost
            };
        }
    }
}
