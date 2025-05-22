using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PosLibrary.Data;
using PosLibrary.Services;
using PosLibrary.Models;

namespace PosApplication
{
    static class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Програмын үндсэн оролт
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                   
            try
            {
                CleanupDatabaseFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Warning: Error cleaning up database files: {ex.Message}", 
                    "Cleanup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Үйлчилгээний тохиргоог хийх
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            
            // Нэвтрэх форм
            var loginForm = new LoginForm();
            Application.Run(loginForm);
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        DbInitializer.Initialize(dbContext).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database initialization error: {ex.Message}", 
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CleanupDatabaseFiles()
        {
            string[] filesToDelete = { 
                "pos.db", 
                "pos.db-shm", 
                "pos.db-wal" 
            };

            foreach (var file in filesToDelete)
            {
                try
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception)
                {
                
                }
            }
        }

        public static void ConfigureServices(ServiceCollection services)
        {
            // DbContext тохиргоо
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=pos.db")
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging()
                      .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log)));

            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<SaleService>();
            services.AddScoped<ReceiptService>();
        }
    }
}
