using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using AdminDashboard.DbStuff.Interfaces;

namespace AdminDashboard.DbStuff.Repositories
{
    public class BaseRepository<DbModel> : IAddEntity<DbModel> where DbModel : BaseModel
    {
        protected readonly SocialNetworkWebDbContext _context;
        protected readonly DbSet<DbModel> _entyties;

        public BaseRepository(SocialNetworkWebDbContext context)
        {
            _context = context;
            _entyties = _context.Set<DbModel>();
        }

        public int Add(DbModel entity)
        {
            _entyties.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public virtual DbModel? GetById(int id)
        {
            return _entyties.SingleOrDefault(ent => ent.Id == id);
        }
    }
}
