using BU.OnlineShop.CatalogService.Domain.Shared;
using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace BU.OnlineShop.CatalogService.Domain.Shared.Categories
{
    public class CategoryNotFoundException : ExceptionBase
    {
        public CategoryNotFoundException(string message)
        {
            Code = CatalogServiceErrorCodes.CategoryNotFoundException;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
