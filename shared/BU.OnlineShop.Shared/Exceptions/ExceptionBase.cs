using System.Net;
using System.Text.Json;

namespace BU.OnlineShop.Shared.Exceptions
{
    public class ExceptionBase : Exception
    {
        public HttpStatusCode? StatusCode { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(new
            {
                StatusCode = StatusCode,
                Code = Code,
                Message = Message
            });
        }
    }
}
