
namespace MobileTopup.Infrastructure.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        List<TEntity> GetAll();
        TEntity GetById(int id);
        void Update(TEntity entity);
    }
}