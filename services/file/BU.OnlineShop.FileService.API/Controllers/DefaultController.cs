using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.FileService.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        [Route("/")]
        [Route("/docs")]
        [Route("/swagger")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
