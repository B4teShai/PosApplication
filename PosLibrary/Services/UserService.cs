using Microsoft.EntityFrameworkCore;
using PosLibrary.Data;
using PosLibrary.Models;

namespace PosLibrary.Services
{
    /// <summary>
    /// POS системд хэрэглэгчдийг удирдах үйлчилгээ үзүүлдэг.
    /// </summary>
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// UserService классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        /// <param name="context">Хэрэглэгчийн үйлдлийн өгөгдлийн сангийн контекст.</param>
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Өгөгдсөн нэр болон нууц үгээр хэрэглэгчийг баталгаажуулна.
        /// </summary>
        /// <param name="username">Баталгаажуулах хэрэглэгчийн нэр.</param>
        /// <param name="password">Баталгаажуулах нууц үг.</param>
        /// <returns>Амжилттай бол баталгаажсан хэрэглэгчийг буцаана; эсвэл, null.</returns>
        public async Task<User?> AuthenticateUser(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => 
                    u.Username.ToLower() == username.ToLower() && 
                    u.Password == password);
        }

        /// <summary>
        /// Системд шинэ хэрэглэгч үүсгэнэ.
        /// </summary>
        /// <param name="user">Үүсгэх хэрэглэгч.</param>
        /// <returns>Хэрэглэгч амжилттай үүссэн бол true; эсвэл, false.</returns>
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Системээс бүх хэрэглэгчдийг татаж авна.
        /// </summary>
        /// <returns>Систем дэх бүх хэрэглэгчдийн жагсаалт.</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Систем дэх байгаа хэрэглэгчийг шинэчилнэ.
        /// </summary>
        /// <param name="user">Шинэчлэх хэрэглэгч.</param>
        /// <returns>Хэрэглэгч амжилттай шинэчлэгдсэн бол true; эсвэл, false.</returns>
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Системээс хэрэглэгчийг устгана.
        /// </summary>
        /// <param name="userId">Устгах хэрэглэгчийн ID.</param>
        /// <returns>Хэрэглэгч амжилттай устгагдсан бол true; эсвэл, false.</returns>
        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Хэрэв хэрэглэгч байхгүй бол системийг анхдагч хэрэглэгчдээр эхлүүлнэ.
        /// </summary>
        /// <returns> Async operation </returns>
        public async Task InitializeDefaultUsers()
        {
            if (!await _context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new Manager
                    {
                        Username = "manager",
                        Password = "manager123",
                        FullName = "System Manager",
                        Role = UserRole.Manager
                    },
                    new Cashier
                    {
                        Username = "cashier1",
                        Password = "cashier123",
                        FullName = "Cashier 1",
                        Role = UserRole.Cashier1
                    },
                    new Cashier
                    {
                        Username = "cashier2",
                        Password = "cashier123",
                        FullName = "Cashier 2",
                        Role = UserRole.Cashier2
                    }
                };

                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
        }
    }
} 