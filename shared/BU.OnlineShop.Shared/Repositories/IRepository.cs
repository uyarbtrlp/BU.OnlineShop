using BU.OnlineShop.Shared.Entities;

namespace BU.OnlineShop.Shared.Repository
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        Task<TEntity> GetAsync(Guid id);

        Task<TEntity> FindAsync(Guid id);

        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false);

        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false);

        Task DeleteAsync(TEntity entity, bool autoSave = false);

        Task<bool> SaveChangesAsync();
    }
}
