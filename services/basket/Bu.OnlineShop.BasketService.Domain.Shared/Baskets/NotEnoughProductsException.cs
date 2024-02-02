using Bu.OnlineShop.BasketService.Domain.Shared;
using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace BU.OnlineShop.BasketService.Domain.Shared.Baskets
{
    public class NotEnoughProductsException : ExceptionBase
    {
        public NotEnoughProductsException(string message)
        {
            Code = BasketServiceErrorCodes.NotEnoughProductsException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
