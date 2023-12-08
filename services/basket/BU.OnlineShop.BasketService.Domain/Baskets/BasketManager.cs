namespace BU.OnlineShop.BasketService.Domain.Baskets
{
    public class BasketManager : IBasketManager
    {
        private readonly IBasketRepository _basketRepository;
        public BasketManager(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Basket> CreateAsync(Guid userId)
        {
            var basket = await _basketRepository.FindByUserIdAsync(userId: userId);

            if(basket != null)
            {
                throw new Exception("This user already has the basket");
            }

            return new Basket(userId);
        }
    }
}
