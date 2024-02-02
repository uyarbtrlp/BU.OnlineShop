using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace Bu.OnlineShop.BasketService.Domain.Shared.Baskets
{
    public class PaymentNotSuccessfulException : ExceptionBase
    {
        public PaymentNotSuccessfulException(string message)
        {
            Code = BasketServiceErrorCodes.PaymentNotSuccessfulException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
