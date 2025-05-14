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
        // Make the service provider available globally
        public static ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // First ensure no existing database files are blocking us
            try
            {
                CleanupDatabaseFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Warning: Unable to clean up database files: {ex.Message}", 
                    "Cleanup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            
            // Start with the login form
            var loginForm = new LoginForm();
            Application.Run(loginForm);
        }

        // Helper method to initialize the database when needed
        public static void InitializeDatabase()
        {
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        // Ensure database is created and seeded
                        DbInitializer.Initialize(dbContext).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while initializing the database: {ex.Message}", 
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Service error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to clean up database files
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
                    // Ignore exceptions on cleanup
                }
            }
        }

        public static void ConfigureServices(ServiceCollection services)
        {
            // Configure DbContext 
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=pos.db")
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging()
                      .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log)));

            // Register services
            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<SaleService>();
            services.AddScoped<ReceiptService>();

        }
    }
}
