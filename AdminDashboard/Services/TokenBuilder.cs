using AdminDashboard.DbStuff.Models;
using AdminDashboard.Services.Interfaces;

namespace AdminDashboard.Services
{
    public class TokenBuilder : IService
    {
        public Token BuildToken(string? NameToken, float rate)
        {
            return new Token()
            {
                NameToken = NameToken,
                Rate = rate
            };
        }
    }
}
