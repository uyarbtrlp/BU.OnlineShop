using BU.OnlineShop.Shared.Extensions;

namespace BU.OnlineShop.BasketService.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CompleteAsync(string cardNumber, string cardName, string cardExpiration, float totalPrice)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/payment-service/complete", new { cardNumber, cardName, cardExpiration, totalPrice });
            return await response.ReadContentAs<bool>();
        }
    }
}
