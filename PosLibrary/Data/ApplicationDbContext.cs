using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using PosLibrary.Models;
using System.Linq;

namespace PosLibrary.Data
{
    /// <summary>
    /// POS системд өгөгдлийн сангийн контекст үүсгэнэ.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// ApplicationDbContext классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        /// <param name="options">Контекстыг тогтмол болгохын тулд ашиглагдана.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Parameterless constructor for design-time factory
        /// </summary>
        public ApplicationDbContext()
            : base(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PosDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options)
        {
        }

        /// <summary>
        /// Хэрэглэгчдийн жагсаалтыг авах.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Бүтээгдэхүүнүүддийн жагсаалтыг авах.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Ангиллын жагсаалтыг авах.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Борлуулалтын жагсаалтыг авах.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Борлуулалтын бүтээгдэхүүнүүддийн жагсаалтыг авах.
        /// </summary>
        public DbSet<SaleItem> SaleItems { get; set; }

        /// <summary>
        /// Сагсны жагсаалтыг авах.
        /// </summary>
        public DbSet<Cart> Carts { get; set; }

        /// <summary>
        /// Сагсны бүтээгдэхүүнүүддийн жагсаалтыг авах.
        /// </summary>
        public DbSet<CartItem> CartItems { get; set; }

        /// <summary>
        /// Тохируулах модель үүсгэнэ.
        /// </summary>
        /// <param name="modelBuilder">Тохируулах модель үүсгэнэ.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator(u => u.Role)
                .HasValue<Manager>(UserRole.Manager)
                .HasValue<Cashier>(UserRole.Cashier1);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            SeedDefaultUsers(modelBuilder);
        }

        private void SeedDefaultUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = 1,
                    Username = "manager",
                    Password = "manager123",
                    FullName = "System Manager",
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = System.DateTime.Now
                }
            );

            modelBuilder.Entity<Cashier>().HasData(
                new Cashier
                {
                    Id = 2,
                    Username = "cashier1",
                    Password = "cashier123",
                    FullName = "Cashier 1",
                    Role = UserRole.Cashier1,
                    IsActive = true,
                    CreatedAt = System.DateTime.Now
                },
                new Cashier
                {
                    Id = 3,
                    Username = "cashier2",
                    Password = "cashier123",
                    FullName = "Cashier 2",
                    Role = UserRole.Cashier2,
                    IsActive = true,
                    CreatedAt = System.DateTime.Now
                }
            );
        }
    }
} 