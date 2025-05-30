using System;
using System.Windows.Forms;
using PosLibrary.Models;
using PosLibrary.Services;
using PosLibrary.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PosApplication
{
    public partial class PaymentForm : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;
        private readonly decimal _totalAmount;
        private readonly ReceiptService _receiptService;

        public PaymentForm(Cart cart)
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=pos.db")
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log))
                .Options;
            _context = new ApplicationDbContext(options);
            _receiptService = new ReceiptService(_context);
            _cart = cart;
            _totalAmount = cart.Total;
            InitializeData();
        }

        private void InitializeData()
        {
            totalLabel.Text = $"Total Amount: ${_cart.Total:F2}";
            amountPaidNumericUpDown.Minimum = _cart.Total;
            amountPaidNumericUpDown.Value = _cart.Total;
            UpdateChange();
        }

        private void amountPaidNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateChange();
        }

        private void UpdateChange()
        {
            decimal change = amountPaidNumericUpDown.Value - _totalAmount;
            changeLabel.Text = $"Change: ${change:F2}";
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            _cart.AmountPaid = amountPaidNumericUpDown.Value;
            _cart.IsPaid = true;
            _cart.PaidAt = DateTime.Now;
            
            try
            {
                await PrintReceipt();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task PrintReceipt()
        {
            string textPath = null;
            
            try
            {
                var receipt = await _receiptService.GenerateReceipt(_cart);
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string receiptFolder = Path.Combine(documentsPath, "POS_Receipts");
                
                if (!Directory.Exists(receiptFolder))
                {
                    Directory.CreateDirectory(receiptFolder);
                }
                
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                textPath = Path.Combine(receiptFolder, $"Receipt_{timestamp}.txt");
                
                File.WriteAllText(textPath, receipt);
                
                StringBuilder purchasedItems = new StringBuilder();
                purchasedItems.AppendLine("Purchase Summary:");
                purchasedItems.AppendLine("------------------");
                
                foreach (var item in _cart.Items)
                {
                    purchasedItems.AppendLine($"{item.Quantity} x {item.ProductName} - ${item.UnitPrice:F2} each = ${item.Subtotal:F2}");
                }
                
                purchasedItems.AppendLine("------------------");
                purchasedItems.AppendLine($"Total: ${_cart.Total:F2}");
                purchasedItems.AppendLine($"Amount Paid: ${_cart.AmountPaid:F2}");
                purchasedItems.AppendLine($"Change: ${_cart.AmountPaid - _cart.Total:F2}");
                
                MessageBox.Show(
                    $"{purchasedItems}\n\nReceipt has been saved to:\n{textPath}", 
                    "Sale Completed Successfully", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"File error while generating receipt: {ioEx.Message}\n\n" +
                    "The payment has been processed, but the receipt could not be saved.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating receipt: {ex.Message}\n\n" +
                    "The payment has been processed, but the receipt could not be generated.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                Debug.WriteLine($"Receipt error details: {ex}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _context.Dispose();
        }
    }
} 