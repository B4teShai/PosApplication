using PosLibrary.Models;
using System.Collections.Generic;

namespace PosLibrary.Services
{
    /// <summary>
    /// Service for managing shopping cart operations
    /// </summary>
    public class CartService
    {
        private Cart _currentCart;

        /// <summary>
        /// Initializes a new instance of the CartService class
        /// </summary>
        public CartService()
        {
            _currentCart = new Cart
            {
                Items = new List<CartItem>()
            };
        }

        /// <summary>
        /// Gets the current cart
        /// </summary>
        /// <returns>The current cart</returns>
        public Cart GetCart()
        {
            return _currentCart;
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        /// <param name="product">The product to add</param>
        /// <param name="quantity">The quantity to add</param>
        /// <returns>True if successful</returns>
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
        /// Updates the quantity of an item in the cart
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="quantity">The new quantity</param>
        /// <returns>True if successful</returns>
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
        /// Removes an item from the cart
        /// </summary>
        /// <param name="productId">The product ID to remove</param>
        /// <returns>True if successful</returns>
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
        /// Clears all items from the cart
        /// </summary>
        public void ClearCart()
        {
            _currentCart.Items.Clear();
            _currentCart.Total = 0;
            _currentCart.AmountPaid = 0;
        }

        /// <summary>
        /// Updates the cart total based on item subtotals
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