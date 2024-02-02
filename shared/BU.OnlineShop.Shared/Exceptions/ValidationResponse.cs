using System.Net;

namespace BU.OnlineShop.Shared.Exceptions
{
    public class ValidationResponse
    {
        public HttpStatusCode? StatusCode { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public List<string> ValidationErrors { get; set; }
    }
}
