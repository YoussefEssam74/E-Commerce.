using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presntation.Attributes
{
    class CacheAttribute(int DurationInSeconds = 90) : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create cache key
            string cacheKey = CreateCacheKey(context.HttpContext.Request);


            // Search for value using cache key
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await cacheService.GetAsync(cacheKey);

            // Return cached value if found
            if (cacheValue is not null)
            {
                context.Result = new ContentResult
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // Continue to the next action
            var executedContext = await next.Invoke();

            // Store the result in cache if successful
            if (executedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(
                    cacheKey,
                    result.Value!,
                    TimeSpan.FromSeconds(DurationInSeconds)
                );
            }
        }


        private string CreateCacheKey(HttpRequest request)
        {
            // {{BaseUrl}}/api/Products? TypeId=20&BarndId=10
            StringBuilder Key = new StringBuilder();
            Key.Append(value: request.Path + '?');
            foreach (var Item in request.Query.OrderBy(keySelector: Q => Q.Key))
            {
                Key.Append($"{Item.Key}={Item.Value}&");
            }
            return Key.ToString();



        }
    }
}
