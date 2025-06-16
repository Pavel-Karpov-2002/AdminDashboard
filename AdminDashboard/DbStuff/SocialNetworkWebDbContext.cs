using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.DbStuff
{
    public class SocialNetworkWebDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public SocialNetworkWebDbContext(DbContextOptions<SocialNetworkWebDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(user => user.TokenBalance)
                .WithOne(balance => balance.User);
            builder.Entity<User>()
                .HasMany(user => user.Payments)
                .WithOne(payment => payment.User);
            builder.Entity<UserToken>()
                .HasOne(userToken => userToken.Token)
                .WithMany(token => token.UserTokens);
        }
    }
}
