using Azure;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Web.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorResponse(ActionContext Context)
        {
            var Errors = Context.ModelState.Where(predicate: M => M.Value.Errors.Any())
            .Select(selector: M => new ValidationError()
            {
                Field = M.Key,
                Errors = M.Value.Errors.Select(E => E.ErrorMessage)
            });
            var Response = new ValidationErrorToReturn()
            {
                ValidationErrors = Errors
            };
            return new BadRequestObjectResult(Response);
        }
    }
}
 
