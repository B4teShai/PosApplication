using PosLibrary.Models;
using System.Collections.Generic;

namespace PosLibrary.Services
{
    /// <summary>
    /// Сагс үүсгэх.
    /// </summary>
    public class CartService
    {
        private Cart _currentCart;

        /// <summary>
        /// Сагс үүсгэх.
        /// </summary>
        public CartService()
        {
            _currentCart = new Cart
            {
                Items = new List<CartItem>()
            };
        }

        /// <summary>
        /// Сагсыг авна.
        /// </summary>
        /// <returns>Сагс</returns>
        public Cart GetCart()
        {
            return _currentCart;
        }

        /// <summary>
        /// Бүтээгдэхүүн нэмэх.
        /// </summary>
        /// <param name="product">Бүтээгдэхүүн</param>
        /// <param name="quantity">Тоо</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public bool AddToCart(Product product, int quantity = 1)
        {
            if (product == null || quantity <= 0)
                return false;

            var existingItem = _currentCart.Items.Find(i => i.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Subtotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                _currentCart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    Subtotal = product.Price * quantity
                });
            }

            UpdateCartTotal();
            return true;
        }

        /// <summary>
        /// Бүтээгдэхүүн тоог шинэчилэх.
        /// </summary>
        /// <param name="productId">Бүтээгдэхүүн ID</param>
        /// <param name="quantity">Шинэ тоо</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public bool UpdateQuantity(int productId, int quantity)
        {
            var item = _currentCart.Items.Find(i => i.ProductId == productId);
            
            if (item == null)
                return false;

            if (quantity <= 0)
            {
                _currentCart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
                item.Subtotal = item.Quantity * item.UnitPrice;
            }

            UpdateCartTotal();
            return true;
        }

        /// <summary>
        /// Бүтээгдэхүүн устгах.
        /// </summary>
        /// <param name="productId">Бүтээгдэхүүн ID</param>
        /// <returns>Амжилттай эсэхийг буцаана.</returns>
        public bool RemoveFromCart(int productId)
        {
            var item = _currentCart.Items.Find(i => i.ProductId == productId);
            
            if (item == null)
                return false;

            _currentCart.Items.Remove(item);
            UpdateCartTotal();
            return true;
        }

        /// <summary>
        /// Сагсыг хасах.
        /// </summary>
        public void ClearCart()
        {
            _currentCart.Items.Clear();
            _currentCart.Total = 0;
            _currentCart.AmountPaid = 0;
        }

        /// <summary>
        /// Сагсын нийт нийлбэрийг тооцоолно.
        /// </summary>
        private void UpdateCartTotal()
        {
            _currentCart.Total = 0;
            
            foreach (var item in _currentCart.Items)
            {
                _currentCart.Total += item.Subtotal;
            }
        }
    }
} 