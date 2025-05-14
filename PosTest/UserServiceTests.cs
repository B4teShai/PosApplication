using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PosLibrary.Services;
using PosLibrary.Models;
using PosLibrary.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PosTest
{
    [TestClass]
    public class UserServiceTests
    {
        private ApplicationDbContext _context;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _userService = new UserService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task AuthenticateUser_WithValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var user = new Cashier { Username = "testuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.AuthenticateUser("testuser", "password123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.Username);
        }

        [TestMethod]
        public async Task AuthenticateUser_WithInvalidCredentials_ShouldReturnNull()
        {
            // Act
            var result = await _userService.AuthenticateUser("invalid", "invalid");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateUser_WithValidUser_ShouldReturnTrue()
        {
            // Arrange
            var user = new Cashier { Username = "newuser", Password = "password123" };

            // Act
            var result = await _userService.CreateUser(user);

            // Assert
            Assert.IsTrue(result);
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            Assert.IsNotNull(createdUser);
        }

        [TestMethod]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new Cashier { Username = "user1", Password = "pass1" },
                new Cashier { Username = "user2", Password = "pass2" }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task UpdateUser_WithValidUser_ShouldReturnTrue()
        {
            // Arrange
            var user = new Cashier { Username = "updateuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Password = "newpassword";
            
            // Act
            var result = await _userService.UpdateUser(user);

            // Assert
            Assert.IsTrue(result);
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.AreEqual("newpassword", updatedUser.Password);
        }

        [TestMethod]
        public async Task DeleteUser_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var user = new Cashier { Username = "deleteuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.DeleteUser(user.Id);

            // Assert
            Assert.IsTrue(result);
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public async Task DeleteUser_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _userService.DeleteUser(-1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InitializeDefaultUsers_ShouldCreateDefaultUsers()
        {
            // Act
            await _userService.InitializeDefaultUsers();

            // Assert
            var users = await _context.Users.ToListAsync();
            Assert.AreEqual(3, users.Count);
            Assert.IsTrue(users.Any(u => u.Username == "Manager"));
            Assert.IsTrue(users.Any(u => u.Username == "Cashier1"));
            Assert.IsTrue(users.Any(u => u.Username == "Cashier2"));
        }
    }
} 