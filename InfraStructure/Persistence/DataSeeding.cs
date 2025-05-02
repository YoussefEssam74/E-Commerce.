using DomainLayer.Contracts;
using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
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
                      await  _dbContext.ProductBrands.AddRangeAsync(ProductBrands);

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
                    var Products =await  JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                      await  _dbContext.Products.AddRangeAsync(Products);
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
    }
    
}
