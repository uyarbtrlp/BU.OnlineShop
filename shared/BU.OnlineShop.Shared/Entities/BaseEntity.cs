namespace BU.OnlineShop.Shared.Entities
{
    public class BaseEntity : IBaseEntity
    {
        //TODO: CreatorId, LastModificationTime, LastModifierId

        public virtual Guid Id { get; protected set; } = default;
        public virtual DateTime CreationTime { get; protected set; } = DateTime.Now;

        protected BaseEntity()
        {

        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }
    }
}
