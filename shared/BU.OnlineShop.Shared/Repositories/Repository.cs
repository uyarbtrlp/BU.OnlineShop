using BU.OnlineShop.Shared.Entities;
using BU.OnlineShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BU.OnlineShop.Shared.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false)
        {
            var newEntity = await _dbContext.Set<TEntity>().AddAsync(entity);

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return newEntity.Entity;
        }

        public virtual async Task<TEntity> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] including)
        {
            var query =  _dbContext.Set<TEntity>().AsQueryable();

            if (including != null)
                including.ToList().ForEach(include =>
                {
                    if (include != null)
                        query = query.Include(include);
                });
            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(id, nameof(entity));
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            _dbContext.Attach(entity);

            var updatedEntity = _dbContext.Set<TEntity>().Update(entity).Entity;

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return updatedEntity;
        }

        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false)
        {
            _dbContext.Set<TEntity>().Remove(entity);

            if (autoSave)
            {
                await SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }


    }
}
