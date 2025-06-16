using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.Services
{
    public class TokenBuilder
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
