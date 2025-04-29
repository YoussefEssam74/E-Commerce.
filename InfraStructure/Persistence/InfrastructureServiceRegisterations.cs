

using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public static class InfrastructureServiceRegisterations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration Configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataSeeding, DataSeeding>();
            return services;
        }
    }
}
