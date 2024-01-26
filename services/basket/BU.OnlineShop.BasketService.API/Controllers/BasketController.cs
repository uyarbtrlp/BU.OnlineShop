using AutoMapper;
using Bu.OnlineShop.OrderingService.Abstractions;
using BU.OnlineShop.BasketService.API.Dtos.Baskets;
using BU.OnlineShop.BasketService.API.Dtos.CatalogService;
using BU.OnlineShop.BasketService.API.Services;
using BU.OnlineShop.BasketService.Baskets;
using BU.OnlineShop.Integration.MessageBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BU.OnlineShop.BasketService.API.Controllers
{
    [Route("api/basket-service/baskets")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketManager _basketManager;
        private readonly ICatalogService _catalogService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public BasketController(IBasketRepository basketRepository, IBasketManager basketManager, ICatalogService catalogService, IPaymentService paymentService, IMapper mapper, IMessageBus messageBus, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _basketManager = basketManager;
            _catalogService = catalogService;
            _paymentService = paymentService;
            _mapper = mapper;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<BasketDto> CreateAsync(CreateInput input)
        {
            var basket = await _basketManager.CreateAsync(input.UserId);

            await _basketRepository.InsertAsync(basket, true);

            return _mapper.Map<BasketDto>(basket);

        }


        [HttpGet]
        [Route("{userId}")]
        public async Task<BasketDto> GetByUserIdAsync(Guid userId)
        {
            var basket = await _basketRepository.GetByUserIdAsync(userId);

            return await GetBasketDtoAsync(basket);

        }

        [HttpGet]
        [Route("exists/{userId}")]
        public async Task<bool> ExistAsync(Guid userId)
        {
            var exist = await _basketRepository.ExistAsync(userId);

            return exist;

        }

        [HttpPost]
        [Route("add-product")]
        public async Task<BasketDto> AddProductAsync(AddProductInput input)
        {
            var basket = await _basketRepository.GetByUserIdAsync(input.UserId);
            var product = await _catalogService.GetAsync(input.ProductId);

            var basketProductCount = basket.GetProductCount(product.Id);

            if (basketProductCount + input.Count > product.StockCount)
            {
                throw new Exception("Not enough porducts!");
            }

            basket.AddProduct(product.Id, input.Count);

            await _basketRepository.UpdateAsync(basket, true);

            return await GetBasketDtoAsync(basket);

        }

        [HttpPost]
        [Route("remove-product")]
        public async Task<BasketDto> RemoveProductAsync(RemoveProductInput input)
        {
            var basket = await _basketRepository.GetByUserIdAsync(input.UserId);

            var product = await _catalogService.GetAsync(input.ProductId);

            basket.RemoveProduct(product.Id, input.Count);

            await _basketRepository.UpdateAsync(basket, true);

            return await GetBasketDtoAsync(basket);
        }

        [HttpPost]
        [Route("checkout")]
        public async Task CheckoutAsync(CheckoutInput input)
        {
            var basket = await _basketRepository.GetByUserIdAsync(input.UserId);

            var basketDto = await GetBasketDtoAsync(basket);

            if(!basketDto.Items.Any()) {

                throw new Exception("There is no items in the basket!");
            }

            // Payment service sync call from fake external service

            var isSuccessful = await _paymentService.CompleteAsync(input.CardNumber, input.CardName, input.CardExpiration, basketDto.TotalPrice);

            if (isSuccessful)
            {
                // Send order async call
                await _messageBus.PublishMessageAsync(new OrderEto()
                {
                    UserId = input.UserId,
                    CreationTime = DateTime.Now,
                    Total = basketDto.TotalPrice,
                    Items = _mapper.Map<List<OrderItemEto>>(basketDto.Items),
                }, OrderingServiceEventBusConsts.QueueName, OrderingServiceEventBusConsts.SendOrderRoutingKey);

            }
            else
            {
                throw new Exception("Payment is not successfull. Please try again!");
            }

            basket.DeleteAllProducts();

            await _basketRepository.UpdateAsync(basket, true);
        }


        private async Task<BasketDto> GetBasketDtoAsync(Basket basket)
        {
            var products = new Dictionary<Guid, ProductDto>();
            var basketDto = new BasketDto();
            var basketChanged = false;

            foreach (var basketItem in basket.BasketLines)
            {
                var productDto = products.GetValueOrDefault(basketItem.ProductId);
                if (productDto == null)
                {
                    productDto = await _catalogService.GetAsync(basketItem.ProductId);
                    products[productDto.Id] = productDto;
                }

                //Removing the products if not available in the stock
                if (basketItem.Count > productDto.StockCount)
                {
                    basket.RemoveProduct(basketItem.ProductId, basketItem.Count - productDto.StockCount);
                    basketChanged = true;
                }

                basketDto.Items.Add(new BasketLineDto
                {
                    ProductId = basketItem.ProductId,
                    Count = basketItem.Count,
                    Code = productDto.Code,
                    Name = productDto.Name,
                    TotalPrice = productDto.Price * basketItem.Count
                });
            }

            basketDto.TotalPrice = basketDto.Items.Sum(x => x.TotalPrice);

            if (basketChanged)
            {
                await _basketRepository.UpdateAsync(basket);
            }

            return basketDto;
        }
    }
}
