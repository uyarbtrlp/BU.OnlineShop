using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.PaymentService.API.Controllers
{
    [Route("api/payment-service")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        [HttpPost]
        public async Task<bool> CompleteAsync(CompleteInput input)
        {
            return await Task.FromResult(true);

        }
    }
}
