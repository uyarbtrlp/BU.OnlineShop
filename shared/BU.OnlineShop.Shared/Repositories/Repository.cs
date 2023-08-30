using BU.OnlineShop.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.Shared.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false)
        {
            var newEntity = await _dbContext.Set<TEntity>().AddAsync(entity);

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return newEntity.Entity;
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return entity;
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            _dbContext.Attach(entity);

            var updatedEntity = _dbContext.Set<TEntity>().Update(entity).Entity;

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return updatedEntity;
        }

        public async Task DeleteAsync(TEntity entity, bool autoSave = false)
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
