using Microsoft.EntityFrameworkCore;
using PosLibrary.Data;
using PosLibrary.Models;

namespace PosLibrary.Services
{
    /// <summary>
    /// POS системд борлуулалтын гүйлгээг удирдах үйлчилгээ үзүүлдэг.
    /// </summary>
    public class SaleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductService _productService;

        /// <summary>
        /// SaleService классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        /// <param name="context">Борлуулалтын үйлдлийн өгөгдлийн сангийн контекст.</param>
        /// <param name="productService">Бүтээгдэхүүнийг удирдах үйлчилгээ.</param>
        public SaleService(ApplicationDbContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        /// <summary>
        /// SaleService классын шинэ жишээг эхлүүлнэ, ProductService-ийг дотооддоо үүсгэнэ.
        /// </summary>
        /// <param name="context">Борлуулалтын үйлдлийн өгөгдлийн сангийн контекст.</param>
        public SaleService(ApplicationDbContext context)
        {
            _context = context;
            _productService = new ProductService(context);
        }

        /// <summary>
        /// Creates a new sale transaction and updates product stock quantities.
        /// </summary>
        /// <param name="sale">The sale transaction to create.</param>
        /// <returns>The created sale transaction.</returns>
        public async Task<Sale> CreateSale(Sale sale)
        {
            try
            {
                // Start a transaction to ensure all updates happen together
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                // Update stock quantities for each item
                foreach (var item in sale.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        // Calculate new stock quantity (don't go below zero)
                        int newStockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
                        
                        // Update product stock directly
                        product.StockQuantity = newStockQuantity;
                        _context.Products.Update(product);
                    }
                }
                
                // Save the stock changes first
                await _context.SaveChangesAsync();
                
                // Then add and save the sale record
                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();
                
                // Commit the transaction
                await transaction.CommitAsync();
                
                return sale;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow to let the caller handle it
                Console.WriteLine($"Error in CreateSale: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Тодорхой өдрийн бүх борлуулалтыг татаж авна.
        /// </summary>
        /// <param name="date">Борлуулалтыг татаж авах өдөр.</param>
        /// <returns>Тухайн өдрийн борлуулалтын жагсаалт.</returns>
        public async Task<List<Sale>> GetSalesByDate(DateTime date)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Include(s => s.User)
                .Where(s => s.CreatedAt.Date == date.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Тодорхой ID-тай борлуулалтыг татаж авна.
        /// </summary>
        /// <param name="saleId">Татаж авах борлуулалтын ID.</param>
        /// <returns>Борлуулалт олдвол буцаана; эсвэл, null.</returns>
        public async Task<Sale?> GetSaleById(int saleId)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == saleId);
        }

        /// <summary>
        /// Борлуулалтын нийт дүнг тооцоолно.
        /// </summary>
        /// <param name="sale">Нийт дүнг тооцоолох борлуулалт.</param>
        /// <returns>Борлуулалтын нийт дүнг буцаана.</returns>
        public decimal CalculateTotal(Sale sale)
        {
            decimal total = 0;
            foreach (var item in sale.Items)
            {
                total += item.UnitPrice * item.Quantity;
            }
            return total;
        }

        /// <summary>
        /// Хэрэглэгчид өгөх хариултыг тооцоолно.
        /// </summary>
        /// <param name="sale">Хариултыг тооцоолох борлуулалт.</param>
        /// <returns>Өгөх хариултын дүнг буцаана.</returns>
        public decimal CalculateChange(Sale sale)
        {
            return sale.AmountPaid - sale.Total;
        }
    }
} 