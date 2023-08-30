namespace BU.OnlineShop.Shared.Entities
{
    public interface IBaseEntity
    {
        public Guid Id { get; }

        public DateTime CreationTime { get; }
    }
}
