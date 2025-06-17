using AdminDashboard.DbStuff.Interfaces;
using AdminDashboard.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

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

        public virtual DbModel Update(DbModel entity)
        {
            var updatedEntity = _entyties.Update(entity);
            _context.SaveChanges();
            return updatedEntity.Entity;
        }

        public virtual void DeleteById(int id)
        {
            var entity = _entyties.First(x => x.Id == id);
            _entyties.Remove(entity);
            _context.SaveChanges();
        }

        public virtual DbModel? GetById(int id)
        {
            return _entyties.SingleOrDefault(ent => ent.Id == id);
        }

        public virtual void DeleteByEntity<DeleteDbModel>(DeleteDbModel entity) where DeleteDbModel : BaseModel
        {
            var findedEntity = _entyties.First(x => x.Equals(entity));
            _entyties.Remove(findedEntity);
            _context.SaveChanges();
        }

        public virtual IEnumerable<DbModel> GetAll()
        {
            return _entyties.ToList();
        }
    }
}
