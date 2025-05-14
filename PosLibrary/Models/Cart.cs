using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PosLibrary.Models
{
    /// <summary>
    /// Сагс.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Сагс ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Хэрэглэгчийн ID.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>       
        /// Хэрэглэгч.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Цэс.
        /// </summary>
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        /// <summary>
        /// Нийт үнэ.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Төлөв.
        /// </summary>
        public bool IsPaid { get; set; }
        
        /// <summary>
        /// Төлсөн мөнгөн дүн.
        /// </summary>
        public decimal AmountPaid { get; set; }
        
        /// <summary>
        /// Төлсөн огноо.
        /// </summary>
        public DateTime? PaidAt { get; set; }
        
        /// <summary>
        /// Үүсгэсэн огноо.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Сагсны бараа.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Сагсны бараа ID.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Сагсны ID.
        /// </summary>
        public int? CartId { get; set; }
        
        /// <summary>
        /// Сагс.
        /// </summary>
        public Cart Cart { get; set; }
        
        /// <summary>
        /// Бараа ID.
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// Бараа.
        /// </summary>
        public Product Product { get; set; }
        
        /// <summary>
        /// Барааны нэр.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Нэгж үнэ.
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Тоо ширхэг.
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Дэд дүн.
        /// </summary>
        public decimal Subtotal { get; set; }
    }
}