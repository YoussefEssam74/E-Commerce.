using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Web.Extentions
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
           
            Services.AddEndpointsApiExplorer();
           
            Services.AddSwaggerGen();

            return Services;
        }

        public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>((Options) =>
             {
                 Options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorResponse;
             });
            return Services;

        }

        public static IServiceCollection AddJWTServices(this IServiceCollection services, IConfiguration configuration)


        {


            services.AddAuthentication(options =>


            {


                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;


                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            })


                .AddJwtBearer(options =>


                {


                    options.TokenValidationParameters = new TokenValidationParameters


                    {


                        ValidateIssuer = true,


                        ValidateAudience = true,


                        ValidateLifetime = true,


                        ValidateIssuerSigningKey = true,


                        ValidIssuer = configuration["JWTOptions:Issuer"],


                        ValidAudience = configuration["JWTOptions:Audience"],


                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTOptions:SecretKey"]))


                    };


                });





            return services;


        }

    }
    }
