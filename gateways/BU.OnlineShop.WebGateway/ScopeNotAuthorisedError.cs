using Ocelot.Errors;

namespace BU.OnlineShop.WebGateway
{
    public class ScopeNotAuthorisedError : Error
    {
        public ScopeNotAuthorisedError(string message)
            : base(message, OcelotErrorCode.ScopeNotAuthorizedError, 403)
        {
        }
    }
}
