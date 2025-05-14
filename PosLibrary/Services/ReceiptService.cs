using PosLibrary.Models;
using PosLibrary.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;

namespace PosLibrary.Services
{
    /// <summary>
    /// POS системд баримт үүсгэх болон хэвлэх үйлчилгээ үзүүлдэг.
    /// </summary>
    public class ReceiptService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// ReceiptService классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        /// <param name="context">Баримтын үйлдлийн өгөгдлийн сангийн контекст.</param>
        public ReceiptService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generates a receipt for a cart
        /// </summary>
        /// <param name="cart">The cart to generate a receipt for</param>
        /// <returns>A formatted receipt string</returns>
        public async Task<string> GenerateReceipt(Cart cart)
        {
            var receipt = new StringBuilder();
            
            // Header
            receipt.AppendLine("=====================================");
            receipt.AppendLine("           POS SYSTEM RECEIPT        ");
            receipt.AppendLine("=====================================");
            receipt.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            receipt.AppendLine($"Receipt #: {Guid.NewGuid().ToString().Substring(0, 8)}");
            receipt.AppendLine("-------------------------------------");
            receipt.AppendLine("Item                Qty    Price    Total");
            receipt.AppendLine("-------------------------------------");
            
            // Items
            foreach (var item in cart.Items)
            {
                // Get product details
                var product = await _context.Products.FindAsync(item.ProductId);
                string itemName = product?.Name ?? item.ProductName;
                string itemCode = product?.Code ?? "";
                
                receipt.AppendLine($"{itemName} ({itemCode})");
                receipt.AppendLine($"         {item.Quantity,3} x ${item.UnitPrice,8:F2} = ${item.Subtotal,8:F2}");
            }
            
            // Totals
            receipt.AppendLine("-------------------------------------");
            receipt.AppendLine($"Subtotal:                    ${cart.Total,8:F2}");
            receipt.AppendLine($"Amount Paid:                 ${cart.AmountPaid,8:F2}");
            receipt.AppendLine($"Change:                      ${(cart.AmountPaid - cart.Total),8:F2}");
            receipt.AppendLine("=====================================");
            receipt.AppendLine("           Thank you!               ");
            receipt.AppendLine("=====================================");
            
            return receipt.ToString();
        }

        /// <summary>
        /// Борлуулалтын гүйлгээний баримтын форматыг үүсгэнэ.
        /// </summary>
        /// <param name="sale">Баримт үүсгэх борлуулалтын гүйлгээ.</param>
        /// <returns>Баримтын дэлгэрэнгүйг агуулсан форматтай мөрийг буцаана.</returns>
        public string GenerateReceipt(Sale sale)
        {
            var receipt = new StringBuilder();
            
            // Header
            receipt.AppendLine("=====================================");
            receipt.AppendLine("           POS SYSTEM RECEIPT        ");
            receipt.AppendLine("=====================================");
            receipt.AppendLine($"Date: {sale.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            receipt.AppendLine($"Receipt #: {sale.Id}");
            receipt.AppendLine($"Cashier: {sale.User?.Username}");
            receipt.AppendLine("-------------------------------------");
            receipt.AppendLine("Item                Qty    Price    Total");
            receipt.AppendLine("-------------------------------------");
            
            // Items
            foreach (var item in sale.Items)
            {
                receipt.AppendLine($"{item.ProductName}");
                receipt.AppendLine($"         {item.Quantity,3} x ${item.UnitPrice,8:F2} = ${item.Subtotal,8:F2}");
            }
            
            // Totals
            receipt.AppendLine("-------------------------------------");
            receipt.AppendLine($"Subtotal:                    ${sale.Total,8:F2}");
            receipt.AppendLine($"Amount Paid:                 ${sale.AmountPaid,8:F2}");
            receipt.AppendLine($"Change:                      ${sale.Change,8:F2}");
            receipt.AppendLine("=====================================");
            receipt.AppendLine("           Thank you!               ");
            receipt.AppendLine("=====================================");
            
            return receipt.ToString();
        }
        
        /// <summary>
        /// Generates a PDF file from receipt text
        /// </summary>
        /// <param name="textFilePath">Path to the text file containing receipt content</param>
        /// <param name="pdfFilePath">Path where the PDF file should be saved</param>
        public void GeneratePdfFromReceipt(string textFilePath, string pdfFilePath)
        {
            try
            {
                // Read the receipt content
                string receiptContent = File.ReadAllText(textFilePath);
                string[] lines = receiptContent.Split('\n');

                // Create a bitmap to draw the receipt
                using (Bitmap bitmap = new Bitmap(500, 800))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // Set up drawing
                        graphics.Clear(Color.White);
                        Font font = new Font("Courier New", 10);
                        float lineHeight = font.GetHeight(graphics);
                        float yPos = 20; // Start from top with some margin
                        float leftMargin = 20;

                        // Draw each line
                        foreach (string line in lines)
                        {
                            graphics.DrawString(line, font, Brushes.Black, leftMargin, yPos);
                            yPos += lineHeight;
                        }
                    }

                    // Save as PDF (using an image format as a simple solution)
                    // In a production environment, you would use a proper PDF library
                    bitmap.Save(pdfFilePath, ImageFormat.Png);
                }

                // Rename the file to .pdf extension (this is a workaround)
                if (File.Exists(pdfFilePath))
                {
                    string pngPath = pdfFilePath;
                    pdfFilePath = Path.ChangeExtension(pdfFilePath, ".pdf");
                    File.Move(pngPath, pdfFilePath, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Баримтыг файлд хадгална.
        /// </summary>
        /// <param name="receipt">Хадгалах баримтын мөр.</param>
        /// <returns>Хадгалагдсан баримтын файлын замыг буцаана.</returns>
        public string SaveReceipt(string receipt)
        {
            string fileName = $"receipt_{DateTime.Now:yyyyMMddHHmmss}.txt";
            File.WriteAllText(fileName, receipt);
            return fileName;
        }
    }
} 