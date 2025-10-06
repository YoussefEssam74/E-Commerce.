using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using System.Reflection.Metadata.Ecma335;

namespace E_Commerce.Web.Extentions
{
    public static class WebApplicationRegisteration
    {
        public static async Task<WebApplication> SeedDataBaseAsync(this WebApplication app)
        {
            using var Scoope = app.Services.CreateScope();
            var ObjectOfDataSeeding = Scoope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.DataSeedAsync();
            await ObjectOfDataSeeding.IdentityDataSeedAsync();
            return app;
        }
        public static IApplicationBuilder UseCustomExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleWare(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
