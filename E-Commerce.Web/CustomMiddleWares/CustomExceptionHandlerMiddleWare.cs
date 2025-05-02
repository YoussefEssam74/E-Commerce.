using Azure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Shared.ErrorModels;
using DomainLayer.Exceptions;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next,ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                await HandleNotFoundAsync(httpContext, ex);

            }

        }

        private async Task HandleNotFoundAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "Somthing Went Wrong");

            // Set status Code For Response 
            //httpContext.Response. StatusCode = (int) HttpStatusCode. InternalServerError; 
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            //Set Content Type For Response
            //httpContext.Response.ContentType = "application/json";

            // Response Object 
            var Response = new ErrorToReturn()
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            };




            // Return Object As JSON 
            // var ResponseToReturn = JsonSerializer.Serialize(value: Response);
            //await httpContext.Response.WriteAsync( ResponseToReturn);
            await httpContext.Response.WriteAsJsonAsync(Response);
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                };
                await httpContext.Response.WriteAsJsonAsync(value: Response);

            }
        }
    }
}
