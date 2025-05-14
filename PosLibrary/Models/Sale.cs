using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosLibrary.Models
{
    /// <summary>
    /// POS системд борлуулалтын гүйлгээг илэрхийлнэ.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Sale классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        public Sale()
        {
            CreatedAt = DateTime.Now;
            Items = new List<SaleItem>();
        }

        /// <summary>
        /// Борлуулалтын ID-г авна.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Борлуулалтын огноо.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Борлуулалтын нийт дүн.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Борлуулалтын өгсөн дүн.
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Борлуулалтын үлдэгдэл.
        /// </summary>
        public decimal Change { get; set; }

        /// <summary>
        /// Хэрэглэгчийн ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Хэрэглэгч.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Худалдсан бүтээгдэхүүнүүд.
        /// </summary>
        public List<SaleItem> Items { get; set; }
    }

    /// <summary>
    /// Худалдсан бүтээгдэхүүн.
    /// </summary>
    public class SaleItem
    {
        /// <summary>
        /// Худалдсан бүтээгдэхүүн ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Борлуулалтын ID.
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// Борлуулалт.
        /// </summary>
        public Sale Sale { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн ID.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн нэр.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Бүтээгдэхүүн үлдэгдэл.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн үнэ.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн нийт үнэ.
        /// </summary>
        public decimal Subtotal { get; set; }
    }
} 