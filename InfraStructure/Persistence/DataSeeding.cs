using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,
       UserManager<ApplicationUser> _userManager,
       RoleManager<IdentityRole> _roleManager,
       StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigrations = _dbContext.Database.GetPendingMigrations();
                if ((PendingMigrations).Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {


                    var ProductBrandData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\brands.json");
                    // var ProductBrandData = await File.ReadAllTextAsync(@"..\InfraStructure\Persistence\Data\DataSeed\brands.json");
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);
                    if (ProductBrands is not null && ProductBrands.Any())
                        await _dbContext.ProductBrands.AddRangeAsync(ProductBrands);

                }
                if (!_dbContext.ProductTypes.Any())
                {
                    //read data
                    var ProductTypesData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\types.json");
                    // convrt to c# object
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    // save to database
                    if (ProductTypes is not null && ProductTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);

                }
                if (!_dbContext.Products.Any())
                {
                    var ProductsData = File.OpenRead(@"..\InfraStructure\Persistence\Data\DataSeed\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                        await _dbContext.Products.AddRangeAsync(Products);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during data seeding: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }

        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {

                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Tarek",
                        PhoneNumber = "123456789",
                        UserName = "MohamedTarek"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "123456789",
                        UserName = "SalmaMohamed"
                    };

                    await _userManager.CreateAsync(User01, "P@ssword");
                    await _userManager.CreateAsync(User02, "P@ssword");
                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }
               await _identityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
