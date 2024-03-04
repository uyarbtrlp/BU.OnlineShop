using BU.OnlineShop.Shared.Exceptions;
using System.Net;

namespace BU.OnlineShop.FileService.Domain.Shared.FileInformations
{
    public class FileAlreadyExists : ExceptionBase
    {
        public FileAlreadyExists(string message)
        {
            Code = FileServiceErrorCodes.FileAlreadyExists;
            Message = message;
            StatusCode = HttpStatusCode.Forbidden;
        }
    }
}
