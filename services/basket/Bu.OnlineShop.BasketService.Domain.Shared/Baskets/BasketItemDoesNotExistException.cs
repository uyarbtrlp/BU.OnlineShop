using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace Bu.OnlineShop.BasketService.Domain.Shared.Baskets
{
    public class BasketItemDoesNotExistException : ExceptionBase
    {
        public BasketItemDoesNotExistException(string message)
        {
            Code = BasketServiceErrorCodes.NotEnoughProductsException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
