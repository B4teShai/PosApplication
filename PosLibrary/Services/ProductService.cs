using Microsoft.EntityFrameworkCore;
using PosLibrary.Data;
using PosLibrary.Models;

namespace PosLibrary.Services
{
    /// <summary>
    /// POS системд бүтээгдэхүүн болон ангиллыг удирдах үйлчилгээ үзүүлдэг.
    /// </summary>
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// ProductService классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        /// <param name="context">Бүтээгдэхүүний үйлдлийн өгөгдлийн сангийн контекст.</param>
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Бүтээгдэхүүнийг түүний кодоор татаж авна.
        /// </summary>
        /// <param name="code">Хайх бүтээгдэхүүний код.</param>
        /// <returns>Бүтээгдэхүүн олдвол буцаана; эсвэл, null.</returns>
        public async Task<Product?> GetProductByCode(string code)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        /// <summary>
        /// Системээс бүх бүтээгдэхүүнийг татаж авна.
        /// </summary>
        /// <returns>Систем дэх бүх бүтээгдэхүүний жагсаалт.</returns>
        public async Task<List<Product>> GetAllProducts()
        {
     
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        /// <summary>
        /// Системээс бүх бүтээгдэхүүний ангиллыг татаж авна.
        /// </summary>
        /// <returns>Систем дэх бүх ангиллын жагсаалт.</returns>
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Retrieves all categories asynchronously
        /// </summary>
        /// <returns>A list of all categories</returns>
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Gets products by category ID
        /// </summary>
        /// <param name="categoryId">The category ID to filter by</param>
        /// <returns>A list of products in the specified category</returns>
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        /// <summary>
        /// Системд шинэ бүтээгдэхүүн нэмнэ.
        /// </summary>
        /// <param name="product">Нэмэх бүтээгдэхүүн.</param>
        /// <returns>Бүтээгдэхүүн амжилттай нэмэгдсэн бол true; эсвэл, false.</returns>
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Систем дэх байгаа бүтээгдэхүүнийг шинэчилнэ.
        /// </summary>
        /// <param name="product">Шинэчлэх бүтээгдэхүүн.</param>
        /// <returns>Бүтээгдэхүүн амжилттай шинэчлэгдсэн бол true; эсвэл, false.</returns>
        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Системээс бүтээгдэхүүнийг устгана.
        /// </summary>
        /// <param name="productId">Устгах бүтээгдэхүүний ID.</param>
        /// <returns>Бүтээгдэхүүн амжилттай устгагдсан бол true; эсвэл, false.</returns>
        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
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
        /// Ангиллыг нэмнэ.
        /// </summary>
        /// <param name="category">Ангиллыг нэмэх.</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ангиллыг шинэчилэх.
        /// </summary>
        /// <param name="category">Ангиллыг шинэчилэх.</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ангиллыг устгана.
        /// </summary>
        /// <param name="categoryId">Ангиллын ID</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                bool hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
                if (hasProducts)
                {
                    return false;
                }

                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
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
        /// Систем дэх бүтээгдэхүүний нөөцийн хэмжээг шинэчилнэ.
        /// </summary>
        /// <param name="productId">Шинэчлэх бүтээгдэхүүний ID.</param>
        /// <param name="stockQuantity">Бүтээгдэхүүний шинэ нөөцийн хэмжээ.</param>
        /// <returns>Нөөцийн хэмжээ амжилттай шинэчлэгдсэн бол true; эсвэл, false.</returns>
        public async Task<bool> UpdateProductStockQuantity(int productId, int stockQuantity)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.StockQuantity = stockQuantity;
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
        /// Бүтээгдэхүүнийг ID-ээр татаж авна.
        /// </summary>
        /// <param name="productId">Бүтээгдэхүүн ID-г татаж авна.</param>
        /// <returns>Бүтээгдэхүүн эсвэл, null.</returns>
        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
} 