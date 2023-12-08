using BU.OnlineShop.Shared.Entities;

namespace BU.OnlineShop.BasketService.Domain.Baskets
{
    public class BasketLine : BaseEntity
    {
        public Guid BasketId { get; protected set; }
        public Guid ProductId { get; protected set; }
        public int Count { get; protected set; }

        protected BasketLine()
        {

        }

        public BasketLine(Guid basketId, Guid productId, int count = 1)
        {
            BasketId = basketId;
            ProductId = productId;

            SetCount(count);
        }


        public void SetCount(int count)
        {
            if (count < 1)
            {
                throw new ArgumentException($"{nameof(count)} can not be less than 1!");
            }

            Count = count;
        }
    }
}
