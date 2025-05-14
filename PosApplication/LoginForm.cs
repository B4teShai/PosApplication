using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PosLibrary.Services;
using PosLibrary.Models;
using System.Linq;

namespace PosApplication
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly SaleService _saleService;
        private readonly ReceiptService _receiptService;
        private static bool _databaseInitialized = false;
        private System.Windows.Forms.Label lblTitle;

        public LoginForm()
        {
            InitializeComponent();
            
            var serviceProvider = Program.ServiceProvider;
            
            _userService = serviceProvider.GetRequiredService<UserService>();
            _productService = serviceProvider.GetRequiredService<ProductService>();
            _saleService = serviceProvider.GetRequiredService<SaleService>();
            _receiptService = serviceProvider.GetRequiredService<ReceiptService>();
            
            // Initialize database only once
            if (!_databaseInitialized)
            {
                Program.InitializeDatabase();
                _databaseInitialized = true;
            }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Authenticate 
                var user = await _userService.AuthenticateUser(username, password);
                
                if (user != null)
                {
                    var mainForm = new MainForm(user);
                    mainForm.FormClosed += (s, args) => this.Show(); // Show login form again when main form closes
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!\n\nTry these credentials:\n- Username: manager, Password: manager123\n- Username: cashier1, Password: cashier123", 
                        "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!Application.OpenForms.OfType<MainForm>().Any(f => f.Visible))
                {
                    Application.Exit();
                }
            }
        }
    }
} 