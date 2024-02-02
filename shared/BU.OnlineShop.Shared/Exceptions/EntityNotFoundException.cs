using System.Net;

namespace BU.OnlineShop.Shared.Exceptions
{
    public class EntityNotFoundException : ExceptionBase
    {
        public EntityNotFoundException(Guid id, string entityName)
        {
            Code = null;
            Message = $"There is no entity with {id} in {entityName} type.";
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
