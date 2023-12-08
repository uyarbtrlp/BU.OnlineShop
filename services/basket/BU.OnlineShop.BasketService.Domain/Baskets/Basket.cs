using BU.OnlineShop.Shared.Entities;

namespace BU.OnlineShop.BasketService.Domain.Baskets
{
    public class Basket : BaseEntity
    {
        public Guid UserId { get; protected set; }

        public List<BasketLine> BasketLines { get; protected set; }

        protected Basket()
        {
            // Default constructor is needed for ORMs.
        }

        public Basket(Guid userId)
        {
            UserId = userId;

            BasketLines = new List<BasketLine>();
        }

        public void AddProduct(Guid productId, int count = 1)
        {
            if (count < 1)
            {
                throw new ArgumentException($"{nameof(count)} can not be less than 1!");
            }

            var item = BasketLines.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                BasketLines.Add(new BasketLine(Id, productId, count));
            }
            else
            {
                item.SetCount(item.Count + count);
            }
        }

        public void RemoveProduct(Guid productId, int count = 1)
        {
            if (count < 1)
            {
                throw new ArgumentException($"{nameof(count)} can not be less than 1!");
            }

            var item = BasketLines.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if(item.Count <= count)
            {
                BasketLines.Remove(item);
                return;
            }

            item.SetCount(item.Count - count);
        }

        public int GetProductCount(Guid productId)
        {
            var item = BasketLines.FirstOrDefault(p => p.ProductId == productId);
            return item == null ? 0 : item.Count; 
        }

        public void DeleteAllProducts()
        {
            BasketLines.Clear();
        }
    }
}
