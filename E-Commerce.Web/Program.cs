using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using DomainLayer.Contracts;
using Persistence; // Add this using directive

namespace E_Commerce.Web
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add Services To The Container

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            #endregion


            var app = builder.Build();
            var  Scoope =app.Services.CreateScope();
            var ObjectOfDataSeeding=Scoope.ServiceProvider.GetRequiredService<IDataSeeding>();
           await ObjectOfDataSeeding.DataSeedAsync();

            #region  Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
