using BU.OnlineShop.Shared.Entities;
using JetBrains.Annotations;

namespace BU.OnlineShop.CatalogService.Products
{
    public class Product : BaseEntity
    {
        public Guid CategoryId { get; protected set; }

        [NotNull]
        public string Name { get; protected set; }

        [NotNull]
        public string Code { get; protected set; }

        public float Price { get; protected set; }

        public int StockCount { get; protected set; }

        protected Product()
        {
            // Default constructor is needed for ORMs.
        }

        internal Product(
            Guid id,
            Guid categoryId,
            [NotNull] string code,
            [NotNull] string name,
            float price = 0.0f,
            int stockCount = 0)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException($"{code} can not be null, empty or white space!");
            }

            if (code.Length >= ProductConsts.MaxCodeLength)
            {
                throw new ArgumentException($"Product code can not be longer than {ProductConsts.MaxCodeLength}");
            }

            Id = id;
            SetCategoryId(categoryId);
            SetCode(code);
            SetName(name);
            SetPrice(price);
            SetStockCount(stockCount);
        }


        public void SetCategoryId(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(categoryId)} can not be empty!");
            }

            CategoryId = categoryId;
        }

        public void SetCode([NotNull] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException($"{code} can not be null, empty or white space!");
            }

            if (code.Length >= ProductConsts.MaxCodeLength)
            {
                throw new ArgumentException($"Product name can not be longer than {ProductConsts.MaxCodeLength}");
            }

            Code = code;
        }
        public void SetName([NotNull] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name} can not be null, empty or white space!");
            }

            if (name.Length >= ProductConsts.MaxNameLength)
            {
                throw new ArgumentException($"Product name can not be longer than {ProductConsts.MaxNameLength}");
            }

            Name = name;
        }

        public void SetPrice(float price)
        {
            if (price < 0.0f)
            {
                throw new ArgumentException($"{nameof(price)} can not be less than 0.0!");
            }

            Price = price;
        }



        public void SetStockCount(int stockCount)
        {
            if (StockCount < 0)
            {
                throw new ArgumentException($"{nameof(stockCount)} can not be less than 0!");
            }

            StockCount = stockCount;
        }

    }
}
