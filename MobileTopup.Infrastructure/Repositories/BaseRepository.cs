using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationContext dbContext;

        public BaseRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Abstract implementation for GetAll<T> method
        public virtual List<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().AsNoTracking().ToList();
        }

        // Abstract implementation for GetById<T> method
        public virtual TEntity GetById(int id) 
        {
            return dbContext.Set<TEntity>().Find(id);
        }

        // Abstract implementation for Add<T> method
        public virtual void Add(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();
        }

        // Abstract implementation for Update<T> method
        public virtual void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            dbContext.SaveChanges();

        }

        // Abstract implementation for Delete<T> method
        public virtual void Delete(TEntity entity) 
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

    }
}
