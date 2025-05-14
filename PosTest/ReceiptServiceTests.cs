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

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _receiptService = new ReceiptService(_context);
            
            // Create a test sale with all required properties
            _testSale = new Sale
            {
                Id = 1,
                Date = DateTime.Now,
                User = new Cashier { Username = "TestCashier", Password = "TestPassword", Role = "Cashier" },
                Items = new List<SaleItem>
                {
                    new SaleItem
                    {
                        Product = new Product { Name = "Test Product", Code = "TP001" },
                        Quantity = 2,
                        UnitPrice = 10.99m,
                        TotalPrice = 21.98m
                    }
                },
                TotalAmount = 21.98m,
                AmountPaid = 25.00m,
                Change = 3.02m
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void GenerateReceipt_ShouldReturnFormattedReceipt()
        {
            // Act
            var receipt = _receiptService.GenerateReceipt(_testSale);

            // Assert
            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
            Assert.IsTrue(receipt.Contains("Test Product"));
            Assert.IsTrue(receipt.Contains("TestCashier"));
            Assert.IsTrue(receipt.Contains("21.98"));
        }

        [TestMethod]
        public void GenerateReceipt_WithNullUser_ShouldHandleGracefully()
        {
            // Arrange
            _testSale.User = null;

            // Act
            var receipt = _receiptService.GenerateReceipt(_testSale);

            // Assert
            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
        }

        [TestMethod]
        public void GenerateReceipt_WithEmptyItems_ShouldHandleGracefully()
        {
            // Arrange
            _testSale.Items = new List<SaleItem>();

            // Act
            var receipt = _receiptService.GenerateReceipt(_testSale);

            // Assert
            Assert.IsNotNull(receipt);
            Assert.IsTrue(receipt.Contains("POS SYSTEM RECEIPT"));
        }
    }
} 