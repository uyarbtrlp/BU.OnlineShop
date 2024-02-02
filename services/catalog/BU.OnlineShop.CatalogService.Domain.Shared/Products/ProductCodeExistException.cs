using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace BU.OnlineShop.CatalogService.Domain.Shared.Products
{
    public class ProductCodeExistException : ExceptionBase
    {
        public ProductCodeExistException(string message)
        {
            Code = CatalogServiceErrorCodes.ProductCodeExistException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
