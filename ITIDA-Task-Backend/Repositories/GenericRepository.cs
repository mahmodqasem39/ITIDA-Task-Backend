using ITIDATask.DAL;
using ITIDATask.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ITIDATask.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _set;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
            }
            await Task.CompletedTask;
        }


           public async Task UpdateAsync(TEntity entity)
        {
            _set.Update(entity);
            await Task.CompletedTask;
        }
        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }
    }
}
