using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extentions; // Add this using directive
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

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
            builder.Services.AddJWTServices(builder.Configuration);

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
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.ConfigObject=new ConfigObject
                    {
                     DisplayRequestDuration= true
                    };
                    options.DocumentTitle = "My E-Commerce API";

                    options.JsonSerializerOptions=new JsonSerializerOptions
                    {
                        PropertyNamingPolicy= JsonNamingPolicy.CamelCase
                    };

                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                    options.EnablePersistAuthorization();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
