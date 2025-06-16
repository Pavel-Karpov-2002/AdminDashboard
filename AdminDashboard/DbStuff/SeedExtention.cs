using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDashboard.DbStuff
{
    public class SeedExtention
    {
        public const int PAYMENTS_COUNT = 5;
        public const int USER_TOKEN_COUNT = 10;

        public static void Seed(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                SeedToken(serviceScope.ServiceProvider);
                SeedUser(serviceScope.ServiceProvider);
            }
        }

        private static void SeedUser(IServiceProvider serviceProvider)
        {
            var userRepository = serviceProvider.GetService<UserRepository>();
            var tokenRepository = serviceProvider.GetService<TokenRepository>();
            var userBuilder = serviceProvider.GetService<UserBuilder>();
            var userLodachka = userBuilder.BuildUser("Lodachka", "lod@yandex.by", "123456", BuildUserTokensList(userBuilder, tokenRepository), BuildUserPaymentsList(userBuilder, tokenRepository));
            var userPavel = userBuilder.BuildUser("Pavel", "pavel@gmail.com", "123456", BuildUserTokensList(userBuilder, tokenRepository), BuildUserPaymentsList(userBuilder, tokenRepository));
            var admin = userBuilder.BuildUser("admin", "admin@mirra.dev", "admin123", BuildUserTokensList(userBuilder, tokenRepository), BuildUserPaymentsList(userBuilder, tokenRepository));
            userRepository.Add(userLodachka);
            userRepository.Add(userPavel);
            userRepository.Add(admin);
        }

        private static List<UserToken> BuildUserTokensList(UserBuilder userBuilder, TokenRepository tokenRepository)
        {
            List<UserToken> userTokens = new List<UserToken>();
            Random rnd = new Random();
            for (var i = 0; i < USER_TOKEN_COUNT; i++)
            {
                userTokens.Add(userBuilder.BuildUserToken(tokenRepository.GetById(rnd.Next(1, 11)), ((float)rnd.NextDouble() * 10)));
            }
            return userTokens;
        }

        private static List<Payment> BuildUserPaymentsList(UserBuilder userBuilder, TokenRepository tokenRepository)
        {
            List<Payment> userPayments = new List<Payment>();
            Random rnd = new Random();
            for (var i = 0; i < USER_TOKEN_COUNT; i++)
            {
                userPayments.Add(userBuilder.BuildPayment(
                    tokenRepository.GetById(rnd.Next(1, 11)).NameToken, 
                    (float)rnd.NextDouble() * 10, 
                    GetRandomDateTime()));
            }
            return userPayments;
        }

        private static DateTime GetRandomDateTime()
        {
            Random r = new Random();
            int randomYear = r.Next(1990, 2025);   
            int randomMonthNr = r.Next(1, 13);
            int maxDayNr = DateTime.DaysInMonth(randomYear, randomMonthNr);
            int randomDayNr = r.Next(1, (maxDayNr + 1));
            return new DateTime(randomYear, randomMonthNr, randomDayNr);
        }

        private static void SeedToken(IServiceProvider serviceProvider)
        {
            var tokenRepository = serviceProvider.GetService<TokenRepository>();
            var tokenBuilder = serviceProvider.GetService<TokenBuilder>();
            List<Token> tokens = new List<Token>()
            {
                tokenBuilder.BuildToken("BT", 4562.3123f),
                tokenBuilder.BuildToken("Doge", 892.3f),
                tokenBuilder.BuildToken("Eth", 256.3f),
                tokenBuilder.BuildToken("T3", 872.376f),
                tokenBuilder.BuildToken("Tew", 42.334f),
                tokenBuilder.BuildToken("RSDF", 682.3876f),
                tokenBuilder.BuildToken("USDT", 662.2343f),
                tokenBuilder.BuildToken("KOL", 2.3435f),
                tokenBuilder.BuildToken("DDLF", 223.3f),
                tokenBuilder.BuildToken("SDXC", 4352.3f)
            };
            foreach (var token in tokens)
            {
                tokenRepository.Add(token);
            }
        }
    }
}
