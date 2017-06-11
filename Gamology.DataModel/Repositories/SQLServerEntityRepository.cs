using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.DataModel.DatabaseContexts;
using Gamology.DataModel.Models;

namespace Gamology.DataModel.Repositories
{
    public abstract class SQLServerEntityRepository<TEntity> : BaseConnectedRepository<SQLServerDbContext, TEntity>
        where TEntity : class, IModificationHistory
    {
        public abstract DbSet<TEntity> DbSet { get; }

        private SQLServerDbContext _context = new SQLServerDbContext();
        public override SQLServerDbContext DbContext => _context;

        public SQLServerEntityRepository()
            : base()
        {
        }

        public override void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            Save();
        }

        public override TEntity Get(long id)
        {
            return DbSet.Find(id);
        }

        public override IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable<TEntity>();
        }

        public override async Task<IList<TEntity>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<IList<TEntity>> GetAllInMemory()
        {
            if (DbSet.Local.Count == 0)
            {
                return await GetAll().ToListAsync();
            }

            return await Task.Run<IList<TEntity>>(() => DbSet.Local);
        }

        public override TEntity New<TSub>()
        {
            var newEntity = (TEntity)Activator.CreateInstance(typeof(TSub));
            DbSet.Add(newEntity);

            return newEntity;
        }

        public override void Save()
        {
            RemoveEmptyEntities();
            base.Save();
        }

        private void RemoveEmptyEntities()
        {
            for (var i = 0; i < DbSet.Local.Count; i++)
            {
                var entity = DbSet.Local[i];
                if (DbContext.Entry(entity).State == EntityState.Added
                    && entity.IsDirty)
                {
                    DbSet.Remove(entity);
                }
            }
        }
    }
}
