using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected DbContext dbContext;

        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Abstract implementation for GetAll<T> method
        public virtual async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        // Abstract implementation for GetById<T> method
        public virtual async Task<T> GetById<T>(int id) where T : class
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        // Abstract implementation for Add<T> method
        public virtual void Add<T>(T entity) where T : class
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();
        }

        // Abstract implementation for Update<T> method
        public virtual void Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Update(entity);
            dbContext.SaveChanges();

        }

        // Abstract implementation for Delete<T> method
        public virtual void Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }

    }
}
