using Bu.OnlineShop.BasketService.Abstractions;
using BU.OnlineShop.CatalogService.Products;
using MassTransit;
using System.Text.Json;

namespace BU.OnlineShop.CatalogService.Domain.Products
{
    public class BasketEtoConsumer : IConsumer<BasketEto>
    {
        private readonly IProductManager _productManager;

        private readonly IProductRepository _productRepository;

        public BasketEtoConsumer(IProductManager productManager, IProductRepository productRepository)
        {
            _productManager = productManager;
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<BasketEto> context)
        {
            var basketEto = context.Message;

            foreach (var basketItem in basketEto.Items)
            {
                var product = await _productRepository.GetAsync(basketItem.ProductId);

                product.SetStockCount(product.StockCount - basketItem.Count);

                await _productRepository.UpdateAsync(product, true);
            }
        }
    }
}
