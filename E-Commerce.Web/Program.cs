using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using DomainLayer.Contracts;
using Persistence;
using Service.MappingProfiles;
using ServiceAbstraction;
using Service;
using Persistence.Repositories;
using E_Commerce.Web.CustomMiddleWares;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using E_Commerce.Web.Factories;
using E_Commerce.Web.Extentions; // Add this using directive

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add Services To The Container

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            // builder.Services.AddAutoMapper(x=>x.AddProfile(new ProductProfile()));
            // builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();

            #endregion


            var app = builder.Build();
            await app.SeedDataBaseAsync();

            #region  Configure the HTTP request pipeline.


            //app.Use(middleware: async(RequestContext, NextMiddleware)=>
            //{

            //    Console.WriteLine(value: "Request Under Processing");

            //    await NextMiddleware.Invoke();
            //    Console.WriteLine( "Waiting Response");
            //    Console.WriteLine( RequestContext.Response.Body);
            //});
            app.UseCustomExceptionMiddleWare();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
