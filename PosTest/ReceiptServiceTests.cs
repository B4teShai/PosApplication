using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PosLibrary.Services;
using PosLibrary.Models;
using PosLibrary.Data;
using System;
using System.IO;
using System.Collections.Generic;

namespace PosTest
{
    [TestClass]
    public class ReceiptServiceTests
    {
        private ApplicationDbContext _context;
        private ReceiptService _receiptService;
        private Sale _testSale;

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
            _receiptService = new ReceiptService(_context);
            
            _testSale = new Sale
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                User = new Cashier { Username = "TestCashier", Password = "TestPassword", Role = UserRole.Cashier1 },
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = new Product { Name = "Test Product", Code = "TP001" },
                        ProductName = "Test Product",
                        Quantity = 2,
                        UnitPrice = 10.99m,
                        Subtotal = 21.98m
                    }
                },
                Total = 21.98m,
                AmountPaid = 25.00m,
                Change = 3.02m
            };
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
        /// Борлуулалтын баримтыг зөв форматаар үүсгэх ёстой.
        /// </summary>
        [TestMethod]
        public void GenerateReceipt_ShouldReturnFormattedReceipt()
        {
            var receipt = _receiptService.GenerateReceipt(_testSale);

            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
            Assert.IsTrue(receipt.Contains("Test Product"));
            Assert.IsTrue(receipt.Contains("TestCashier"));
            Assert.IsTrue(receipt.Contains("21.98"));
        }

        /// <summary>
        /// Хэрэглэгчийн мэдээлэл байхгүй үед баримтыг зөв үүсгэх ёстой.
        /// </summary>
        [TestMethod]
        public void GenerateReceipt_WithNullUser_ShouldHandleGracefully()
        {
            _testSale.User = null;

            var receipt = _receiptService.GenerateReceipt(_testSale);

            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
        }

        /// <summary>
        /// Борлуулалтын бараа байхгүй үед баримтыг зөв үүсгэх ёстой.
        /// </summary>
        [TestMethod]
        public void GenerateReceipt_WithEmptyItems_ShouldHandleGracefully()
        {
            _testSale.Items = new List<SaleItem>();

            var receipt = _receiptService.GenerateReceipt(_testSale);

            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
        }
    }
}