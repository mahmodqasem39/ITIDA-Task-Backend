using System.Linq.Expressions;

namespace ITIDATask.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> 
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(TEntity entity);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
