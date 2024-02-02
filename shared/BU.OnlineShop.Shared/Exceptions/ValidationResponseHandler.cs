using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.Shared.Exceptions
{
    public class ValidationResponseHandler : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var validationResponse = new BadRequestObjectResult(new ValidationResponse()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Code = null,
                Message = "Your request is not valid!",
                ValidationErrors = context.ModelState.Values.SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage).ToList()
            });
            await validationResponse.ExecuteResultAsync(context);
        }
    }
}
