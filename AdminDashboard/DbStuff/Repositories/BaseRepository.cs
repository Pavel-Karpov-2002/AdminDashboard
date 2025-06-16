using AdminDashboard.DbStuff.Interfaces;
using AdminDashboard.DbStuff.Models;
using System.Collections.Generic;

namespace AdminDashboard.DbStuff.Repositories
{
    public class BaseRepository<DbModel>
    {
        protected readonly SocialNetworkWebDbContext _context;
        protected readonly DbSet<DbModel> _entyties;

        public BaseRepository(SocialNetworkWebDbContext context)
        {
            _context = context;
            _entyties = _context.Set<DbModel>();
        }

        public virtual DbModel? GetById(int id)
        {
            return _entyties.SingleOrDefault(ent => ent.Id == id);
        }
    }
}
