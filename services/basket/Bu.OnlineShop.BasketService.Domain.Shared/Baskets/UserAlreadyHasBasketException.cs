using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace Bu.OnlineShop.BasketService.Domain.Shared.Baskets
{
    public class UserAlreadyHasBasketException : ExceptionBase
    {
        public UserAlreadyHasBasketException(string message)
        {
            Code = BasketServiceErrorCodes.UserAlreadyHasBasketException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
