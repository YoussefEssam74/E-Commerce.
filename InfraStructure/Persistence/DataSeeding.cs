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
                // 1. Create Roles if not exist
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                // 2. Create Users if not exist
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Tarek",
                        PhoneNumber = "123456789",
                        UserName = "MohamedTarek",
                        EmailConfirmed = true
                    };

                    var user02 = new ApplicationUser
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "123456789",
                        UserName = "SalmaMohamed",
                        EmailConfirmed = true
                    };

                    var result1 = await _userManager.CreateAsync(user01, "P@ssword123");
                    var result2 = await _userManager.CreateAsync(user02, "P@ssword123");

                    // Check creation results
                    if (result1.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user01, "Admin");
                    }
                    else
                    {
                        foreach (var error in result1.Errors)
                            Console.WriteLine($"User01 creation error: {error.Description}");
                    }

                    if (result2.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                    }
                    else
                    {
                        foreach (var error in result2.Errors)
                            Console.WriteLine($"User02 creation error: {error.Description}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in IdentityDataSeedAsync: {ex.Message}");
                // Optionally: log ex.StackTrace or inner exception
            }
        }

    }

}