using AdminDashboard.DbStuff.Models;

namespace AdminDashboard.DbStuff.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository(SocialNetworkWebDbContext context) : base(context)
        {
        }
    }
}
