namespace BU.OnlineShop.BasketService.API.Services
{
    public interface IPaymentService
    {
        Task<bool> CompleteAsync(string cardNumber, string cardName, string cardExpiration, float totalPrice);
    }
}
