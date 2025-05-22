using Microsoft.EntityFrameworkCore;
using PosLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace PosLibrary.Data
{
    /// <summary>
    /// Өгөгдлийн сангид жишээ өгөгдөл байгаа эсэхийг шалгана.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Өгөгдлийн сангид жишээ ангиллын жагсаалт болон бүтээгдэхүүнүүд байгаа эсэхийг шалгана.
        /// </summary>
        /// <param name="context">Өгөгдлийн сангийн контекст.</param>
        /// <returns>Асинхрон операцийн тайлбар.</returns>
        public static async Task Initialize(ApplicationDbContext context)
        {
            // Өгөгдлийн сангийн үүсгэн байгаа эсэхийг шалгана.
            await context.Database.EnsureCreatedAsync();

            // Ангиллын жагсаалт байгаа эсэхийг шалгана.
            if (await context.Categories.AnyAsync())
            {
                return; // Өгөгдлийн сангид ангиллын жагсаалт байгаа бол дараагийн ангиллын жагсаалтыг үүсгэхгүй.
            }

            // Ангиллын жагсаалтыг үүсгэж байгаа.
            await SeedCategories(context);
            
            // Бүтээгдэхүүнүүдээ үүсгэж байгаа.
            await SeedProducts(context);
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            // Жишээ ангиллын жагсаалт
            var categories = new List<Category>
            {
                new Category { Name = "Food", Description = "Food items" },
                new Category { Name = "Beverages", Description = "Drinks and beverages" },
                new Category { Name = "Snacks", Description = "Snacks and chips" },
                new Category { Name = "Household", Description = "Household items" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProducts(ApplicationDbContext context)
        {
            // Ангиллын жагсаалтыг үүсгэж байгаа.
            var categories = await context.Categories.ToListAsync();
            if (categories.Count < 4) return;

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");
            Directory.CreateDirectory(basePath);

            // Жишээ бүтээгдэхүүнүүд
            var products = new List<Product>
            {
                new Product { 
                    Code = "F001", 
                    Name = "Bread", 
                    Description = "Fresh bread", 
                    Price = 2.50m, 
                    StockQuantity = 5, 
                    CategoryId = categories[0].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/bread.jpg")
                },
                new Product { 
                    Code = "F002", 
                    Name = "Milk", 
                    Description = "Fresh milk", 
                    Price = 3.99m, 
                    StockQuantity = 30, 
                    CategoryId = categories[0].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/milk.jpg")
                },
                new Product { 
                    Code = "B001", 
                    Name = "Cola", 
                    Description = "Carbonated drink", 
                    Price = 1.99m, 
                    StockQuantity = 100, 
                    CategoryId = categories[1].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/cola.jpg")
                },
                new Product { 
                    Code = "B002", 
                    Name = "Water", 
                    Description = "Mineral water", 
                    Price = 0.99m, 
                    StockQuantity = 150, 
                    CategoryId = categories[1].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/water.jpg")
                },
                new Product { 
                    Code = "S001", 
                    Name = "Chips", 
                    Description = "Potato chips", 
                    Price = 1.50m, 
                    StockQuantity = 75, 
                    CategoryId = categories[2].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/chips.jpg")
                },
                new Product { 
                    Code = "S002", 
                    Name = "Cookies", 
                    Description = "Chocolate cookies", 
                    Price = 2.25m, 
                    StockQuantity = 60, 
                    CategoryId = categories[2].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/cookies.jpg")
                },
                new Product { 
                    Code = "H001", 
                    Name = "Soap", 
                    Description = "Hand soap", 
                    Price = 4.99m, 
                    StockQuantity = 40, 
                    CategoryId = categories[3].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/soap.jpg")
                },
                new Product { 
                    Code = "H002", 
                    Name = "Detergent", 
                    Description = "Laundry detergent", 
                    Price = 8.99m, 
                    StockQuantity = 25, 
                    CategoryId = categories[3].Id,
                    ImagePath = Path.Combine(basePath, "ProductImages/detergent.jpg")
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
} 