using BU.OnlineShop.Shared.Entities;
using JetBrains.Annotations;

namespace BU.OnlineShop.OrderingService.Orders
{
    public class OrderItem : BaseEntity
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public double ProductPrice { get; set; }

        public int ProductCount { get; set; }

        protected OrderItem() 
        { 
        }

        public OrderItem(
            Guid id, 
            Guid productId,
            [NotNull] string productName,
            [NotNull] string productCode,
            [NotNull] double productPrice,
            [NotNull] int productCount)
        {
            Id = id;
            ProductId = productId;

            SetProductName(productName);
            SetProductCode(productCode);
            SetProductPrice(productPrice);
            SetProductCount(productCount);
        }

        public void SetProductName([NotNull] string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException($"{productName} can not be null, empty or white space!");
            }

            if (productName.Length >= OrderConsts.MaxProductNameLength)
            {
                throw new ArgumentException($"Product code can not be longer than {OrderConsts.MaxProductNameLength}");
            }

            ProductName = productName;
        }

        public void SetProductCode([NotNull] string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                throw new ArgumentException($"{productCode} can not be null, empty or white space!");
            }

            if (productCode.Length >= OrderConsts.MaxProductCodeLength)
            {
                throw new ArgumentException($"Product code can not be longer than {OrderConsts.MaxProductCodeLength}");
            }

            ProductCode = productCode;
        }

        public void SetProductCount([NotNull] int productCount)
        {
            if (productCount < 0)
            {
                throw new ArgumentException($"{nameof(productCount)} can not be less than 0!");
            }

            ProductCount = productCount;
        }

        public void SetProductPrice([NotNull] double productPrice)
        {
            if (productPrice < 0)
            {
                throw new ArgumentException($"{nameof(productPrice)} can not be less than 0!");
            }

            ProductPrice = productPrice;
        }



    }
}
