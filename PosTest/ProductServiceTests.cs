using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PosLibrary.Services;
using PosLibrary.Models;
using PosLibrary.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PosTest
{
    [TestClass]
    public class ProductServiceTests
    {
        private ApplicationDbContext _context;
        private ProductService _productService;

        /// <summary>
        /// Тестийн өгөгдлийн сангийн тохиргоог хийж, шаардлагатай үйлчилгээг үүсгэнэ.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _productService = new ProductService(_context);
        }

        /// <summary>
        /// Тестийн өгөгдлийн сангийн мэдээллийг цэвэрлэнэ.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        /// <summary>
        /// Бүх бүтээгдэхүүний жагсаалтыг амжилттай буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetAllProducts_ShouldReturnListOfProducts()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var products = new List<Product>
            {
                new Product { Name = "Product 1", Code = "P001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id },
                new Product { Name = "Product 2", Code = "P002", Price = 20.99m, StockQuantity = 50, CategoryId = category.Id }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetAllProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Хүчинтэй бүтээгдэхүүний кодоор бүтээгдэхүүн олж амжилттай буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetProductByCode_WithValidCode_ShouldReturnProduct()
        {

            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            
            var result = await _productService.GetProductByCode("TP001");

            Assert.IsNotNull(result);
            Assert.AreEqual("TP001", result.Code);
        }

        /// <summary>
        /// Хүчинтэй бус бүтээгдэхүүний кодоор хайхад null утга буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetProductByCode_WithInvalidCode_ShouldReturnNull()
        {
            var result = await _productService.GetProductByCode("INVALID");

            Assert.IsNull(result);
        }

        /// <summary>
        /// Хүчинтэй бүтээгдэхүүн нэмэхэд амжилттай нэмэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task AddProduct_WithValidProduct_ShouldReturnTrue()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "New Product", Code = "NP001", Price = 15.99m, StockQuantity = 75, CategoryId = category.Id };

            var result = await _productService.AddProduct(product);

            Assert.IsTrue(result);
            var addedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Code == "NP001");
            Assert.IsNotNull(addedProduct);
        }

        /// <summary>
        /// Хүчинтэй бүтээгдэхүүний мэдээллийг шинэчлэхэд амжилттай шинэчлэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task UpdateProduct_WithValidProduct_ShouldReturnTrue()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Update Product", Code = "UP001", Price = 25.99m, StockQuantity = 50, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            product.Price = 29.99m;
            
            var result = await _productService.UpdateProduct(product);

            Assert.IsTrue(result);
            var updatedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Code == "UP001");
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(29.99m, updatedProduct.Price);
        }

        /// <summary>
        /// Хүчинтэй бүтээгдэхүүнийг устгахад амжилттай устгагдах ёстой.
        /// </summary>
        [TestMethod]
        public async Task DeleteProduct_WithValidId_ShouldReturnTrue()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Delete Product", Code = "DP001", Price = 35.99m, StockQuantity = 25, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _productService.DeleteProduct(product.Id);

            Assert.IsTrue(result);
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            Assert.IsNull(deletedProduct);
        }

        /// <summary>
        /// Хүчинтэй бус ID-тай бүтээгдэхүүнийг устгахад амжилтгүй болох ёстой.
        /// </summary>
        [TestMethod]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _productService.DeleteProduct(-1);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Хүчинтэй бүтээгдэхүүний үлдэгдэл тоог шинэчлэхэд амжилттай шинэчлэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task UpdateProductStock_WithValidId_ShouldReturnTrue()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _productService.UpdateProductStockQuantity(product.Id, 75);

            Assert.IsTrue(result);
            var updatedProduct = await _context.Products.FindAsync(product.Id);
            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(75, updatedProduct.StockQuantity);
        }

        /// <summary>
        /// Хүчинтэй бус ID-тай бүтээгдэхүүний үлдэгдэл тоог шинэчлэхэд амжилтгүй болох ёстой.
        /// </summary>
        [TestMethod]
        public async Task UpdateProductStock_WithInvalidId_ShouldReturnFalse()
        {
            var result = await _productService.UpdateProductStockQuantity(-1, 100);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Бүх ангилалын жагсаалтыг амжилттай буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetAllCategories_ShouldReturnListOfCategories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Category 1", Description = "Description for Category 1" },
                new Category { Name = "Category 2", Description = "Description for Category 2" }
            };
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            var result = await _productService.GetAllCategories();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }
    }
} 