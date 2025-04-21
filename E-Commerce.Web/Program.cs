using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using DomainLayer.Contracts;
using Persistence;
using Service.MappingProfiles;
using ServiceAbstraction;
using Service; // Add this using directive

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // builder.Services.AddAutoMapper(x=>x.AddProfile(new ProductProfile()));
           // builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
           builder.Services.AddAutoMapper(typeof(Service.AssemblyReference).Assembly);
            builder.Services.AddScoped<IServiceManager,ServiceManager>();

            #endregion


            var app = builder.Build();
            using var  Scoope =app.Services.CreateScope();
            var ObjectOfDataSeeding=Scoope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.DataSeedAsync();

            #region  Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
