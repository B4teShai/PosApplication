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
    public class SaleServiceTests
    {
        private ApplicationDbContext _context;
        private SaleService _saleService;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _productService = new ProductService(_context);
            _saleService = new SaleService(_context, _productService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task CreateSale_WithValidSale_ShouldReturnSale()
        {
            // Arrange
            // First create a category
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            // Create a product
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Create a user
            var user = new Cashier { Username = "TestCashier", Password = "TestPassword", Role = "Cashier" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var sale = new Sale
            {
                Date = DateTime.Now,
                TotalAmount = 10.99m,
                AmountPaid = 20.00m,
                Change = 9.01m,
                UserId = user.Id,
                Items = new List<SaleItem>
                {
                    new SaleItem { ProductId = product.Id, Quantity = 1, UnitPrice = 10.99m }
                }
            };

            // Act
            var result = await _saleService.CreateSale(sale);

            // Assert
            Assert.IsNotNull(result);
            var createdSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == result.Id);
            Assert.IsNotNull(createdSale);
            Assert.AreEqual(1, createdSale.Items.Count);
        }

        [TestMethod]
        public async Task GetSaleById_WithValidId_ShouldReturnSale()
        {
            // Arrange
            // First create a category
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            // Create a product
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Create a user
            var user = new Cashier { Username = "TestCashier", Password = "TestPassword", Role = "Cashier" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var sale = new Sale
            {
                Date = DateTime.Now,
                TotalAmount = 10.99m,
                AmountPaid = 20.00m,
                Change = 9.01m,
                UserId = user.Id,
                Items = new List<SaleItem>
                {
                    new SaleItem { ProductId = product.Id, Quantity = 1, UnitPrice = 10.99m }
                }
            };
            
            // Save the sale directly to the database
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _saleService.GetSaleById(sale.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(sale.Id, result.Id);
            Assert.AreEqual(1, result.Items.Count);
        }

        [TestMethod]
        public async Task GetSaleById_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _saleService.GetSaleById(-1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetSalesByDate_ShouldReturnFilteredSales()
        {
            // Arrange
            // First create a category
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            // Create a product
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Create a user
            var user = new Cashier { Username = "TestCashier", Password = "TestPassword", Role = "Cashier" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var today = DateTime.Now.Date;
            var sales = new List<Sale>
            {
                new Sale
                {
                    Date = today,
                    TotalAmount = 10.99m,
                    AmountPaid = 20.00m,
                    Change = 9.01m,
                    UserId = user.Id,
                    Items = new List<SaleItem>
                    {
                        new SaleItem { ProductId = product.Id, Quantity = 1, UnitPrice = 10.99m }
                    }
                },
                new Sale
                {
                    Date = today.AddDays(-1),
                    TotalAmount = 21.98m,
                    AmountPaid = 25.00m,
                    Change = 3.02m,
                    UserId = user.Id,
                    Items = new List<SaleItem>
                    {
                        new SaleItem { ProductId = product.Id, Quantity = 2, UnitPrice = 10.99m }
                    }
                }
            };
            
            // Save the sales directly to the database
            await _context.Sales.AddRangeAsync(sales);
            await _context.SaveChangesAsync();

            // Act
            var result = await _saleService.GetSalesByDate(today);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(today.Date, result[0].Date.Date);
        }

        [TestMethod]
        public void CalculateTotal_ShouldReturnCorrectTotal()
        {
            // Arrange
            var sale = new Sale
            {
                Items = new List<SaleItem>
                {
                    new SaleItem { Quantity = 2, UnitPrice = 10.99m },
                    new SaleItem { Quantity = 3, UnitPrice = 5.99m }
                }
            };

            // Act
            var result = _saleService.CalculateTotal(sale);

            // Assert
            Assert.AreEqual(39.95m, result); // (2 * 10.99) + (3 * 5.99) = 21.98 + 17.97 = 39.95
        }

        [TestMethod]
        public void CalculateChange_ShouldReturnCorrectChange()
        {
            // Arrange
            var sale = new Sale
            {
                TotalAmount = 25.00m,
                AmountPaid = 30.00m
            };

            // Act
            var result = _saleService.CalculateChange(sale);

            // Assert
            Assert.AreEqual(5.00m, result);
        }
    }
} 