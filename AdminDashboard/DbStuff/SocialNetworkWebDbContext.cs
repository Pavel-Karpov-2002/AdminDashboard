using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.DbStuff
{
    public class SocialNetworkWebDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SocialNetworkWebDbContext(DbContextOptions<SocialNetworkWebDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
