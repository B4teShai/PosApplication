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
            _saleService = new SaleService(_context, _productService);
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
        /// Хүчинтэй борлуулалтын мэдээлэл оруулахад амжилттай үүсгэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task CreateSale_WithValidSale_ShouldReturnSale()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

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

            var result = await _saleService.CreateSale(sale);

            Assert.IsNotNull(result);
            var createdSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == result.Id);
            Assert.IsNotNull(createdSale);
            Assert.AreEqual(1, createdSale.Items.Count);
        }

        /// <summary>
        /// Хүчинтэй борлуулалтын ID-аар хайхад борлуулалтын мэдээлэл олддог ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetSaleById_WithValidId_ShouldReturnSale()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

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
            
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            var result = await _saleService.GetSaleById(sale.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(sale.Id, result.Id);
            Assert.AreEqual(1, result.Items.Count);
        }

        /// <summary>
        /// Хүчинтэй бус борлуулалтын ID-аар хайхад null утга буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetSaleById_WithInvalidId_ShouldReturnNull()
        {
            var result = await _saleService.GetSaleById(-1);

            Assert.IsNull(result);
        }

        /// <summary>
        /// Тухайн огнооны борлуулалтын жагсаалтыг амжилттай буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetSalesByDate_ShouldReturnFilteredSales()
        {
            var category = new Category { Name = "Test Category", Description = "Test Description" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            var product = new Product { Name = "Test Product", Code = "TP001", Price = 10.99m, StockQuantity = 100, CategoryId = category.Id };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

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
            
            await _context.Sales.AddRangeAsync(sales);
            await _context.SaveChangesAsync();

            var result = await _saleService.GetSalesByDate(today);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(today.Date, result[0].Date.Date);
        }

        /// <summary>
        /// Борлуулалтын нийт дүнг зөв тооцоолох ёстой.
        /// </summary>
        [TestMethod]
        public void CalculateTotal_ShouldReturnCorrectTotal()
        {
            var sale = new Sale
            {
                Items = new List<SaleItem>
                {
                    new SaleItem { Quantity = 2, UnitPrice = 10.99m },
                    new SaleItem { Quantity = 3, UnitPrice = 5.99m }
                }
            };

            var result = _saleService.CalculateTotal(sale);

            Assert.AreEqual(39.95m, result); // (2 * 10.99) + (3 * 5.99) = 21.98 + 17.97 = 39.95
        }

        /// <summary>
        /// Борлуулалтын харилцагчид буцаах мөнгийг зөв тооцоолох ёстой.
        /// </summary>
        [TestMethod]
        public void CalculateChange_ShouldReturnCorrectChange()
        {
            var sale = new Sale
            {
                TotalAmount = 25.00m,
                AmountPaid = 30.00m
            };

            var result = _saleService.CalculateChange(sale);

            Assert.AreEqual(5.00m, result);
        }
    }
} 