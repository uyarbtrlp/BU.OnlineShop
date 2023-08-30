namespace BU.OnlineShop.CatalogService.Domain.Entities
{
    public interface IBaseEntity
    {
        public Guid Id { get; }

        public DateTime CreationTime { get; }
    }
}
