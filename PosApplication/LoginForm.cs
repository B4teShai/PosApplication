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
            
            // объектуудыг үүсгэх
            _userService = serviceProvider.GetRequiredService<UserService>();
            _productService = serviceProvider.GetRequiredService<ProductService>();
            _saleService = serviceProvider.GetRequiredService<SaleService>();
            _receiptService = serviceProvider.GetRequiredService<ReceiptService>();
            
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

            // Хэрэглэгчийн нэр болон нууц үгийг шалгах
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Хэрэглэгчийн мэдээллийг шалгах
                var user = await _userService.AuthenticateUser(username, password);
                
                if (user != null)
                {
                    // Үндсэн форм руу шилжих
                    var mainForm = new MainForm(user);
                    mainForm.FormClosed += (s, args) => this.Show();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!\n\nPlease try again.", 
                        "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Гарах товч
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Форм хаагдах
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