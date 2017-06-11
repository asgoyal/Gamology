using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamology.DataModel.Repositories
{
    public abstract class BaseConnectedRepository<TContext, T> : IRepository<TContext, T>
        where T : class
        where TContext : DbContext
    {
        public abstract TContext DbContext { get; }
        public abstract void Delete(T entity);
        public abstract T Get(long id);
        public abstract IQueryable<T> GetAll();
        public abstract T New<TSub>() where TSub : T;
        public abstract Task<IList<T>> GetAllAsync();

        public virtual void Save()
        {
            DbContext.SaveChanges();
        }
    }
}
