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
            _userService = new UserService(_context);
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
        /// Хүчинтэй нэвтрэх мэдээллээр нэвтрэхэд хэрэглэгчийн мэдээлэл буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task AuthenticateUser_WithValidCredentials_ShouldReturnUser()
        {
            var user = new Cashier { Username = "testuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = await _userService.AuthenticateUser("testuser", "password123");

            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.Username);
        }

        /// <summary>
        /// Хүчинтэй бус нэвтрэх мэдээллээр нэвтрэхэд null утга буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task AuthenticateUser_WithInvalidCredentials_ShouldReturnNull()
        {
            var result = await _userService.AuthenticateUser("invalid", "invalid");

            Assert.IsNull(result);
        }

        /// <summary>
        /// Хүчинтэй хэрэглэгчийн мэдээлэл оруулахад амжилттай үүсгэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task CreateUser_WithValidUser_ShouldReturnTrue()
        {
            var user = new Cashier { Username = "newuser", Password = "password123" };

            var result = await _userService.CreateUser(user);

            Assert.IsTrue(result);
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            Assert.IsNotNull(createdUser);
        }

        /// <summary>
        /// Бүх хэрэглэгчдийн жагсаалтыг амжилттай буцаах ёстой.
        /// </summary>
        [TestMethod]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new Cashier { Username = "user1", Password = "pass1" },
                new Cashier { Username = "user2", Password = "pass2" }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var result = await _userService.GetAllUsers();

            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Хүчинтэй хэрэглэгчийн мэдээллийг шинэчлэхэд амжилттай шинэчлэгдэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task UpdateUser_WithValidUser_ShouldReturnTrue()
        {
            var user = new Cashier { Username = "updateuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Password = "newpassword";

            var result = await _userService.UpdateUser(user);

            Assert.IsTrue(result);
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.AreEqual("newpassword", updatedUser.Password);
        }

        /// <summary>
        /// Хүчинтэй хэрэглэгчийг устгахад амжилттай устгагдах ёстой.
        /// </summary>
        [TestMethod]
        public async Task DeleteUser_WithValidId_ShouldReturnTrue()
        {
            var user = new Cashier { Username = "deleteuser", Password = "password123" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = await _userService.DeleteUser(user.Id);

            Assert.IsTrue(result);
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.IsNull(deletedUser);
        }

        /// <summary>
        /// Хүчинтэй бус ID-тай хэрэглэгчийг устгахад амжилтгүй болох ёстой.
        /// </summary>
        [TestMethod]
        public async Task DeleteUser_WithInvalidId_ShouldReturnFalse()
        {
            var result = await _userService.DeleteUser(-1);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Анхны хэрэглэгчдийн мэдээллийг амжилттай үүсгэх ёстой.
        /// </summary>
        [TestMethod]
        public async Task InitializeDefaultUsers_ShouldCreateDefaultUsers()
        {
            await _userService.InitializeDefaultUsers();

            var users = await _context.Users.ToListAsync();
            Assert.AreEqual(3, users.Count);
            Assert.IsTrue(users.Any(u => u.Username == "Manager"));
            Assert.IsTrue(users.Any(u => u.Username == "Cashier1"));
            Assert.IsTrue(users.Any(u => u.Username == "Cashier2"));
        }
    }
} 