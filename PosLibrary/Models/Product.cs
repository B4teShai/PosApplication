using System;
using System.ComponentModel.DataAnnotations;

namespace PosLibrary.Models
{
    /// <summary>
    /// POS системд бүтээгдэхүүн ангиллын модель.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Ангиллын ID-г авна.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ангиллын нэр.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ангиллын тайлбар.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// POS системд бүтээгдэхүүн модель.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Бүтээгдэхүүн ID-г авна.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн код.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Бүтээгдэхүүн нэр.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Бүтээгдэхүүн тайлбар.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Бүтээгдэхүүн үнэ.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн үлдэгдэл.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн үйлчилгээтэй эсэхийг илэрхийлнэ.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Бүтээгдэхүүн үүсгэсэн огноо.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Бүтээгдэхүүн шинэчилсэн огноо.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Ангиллын ID-г авна.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Бүтээгдэхүүн зураг.
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Бүтээгдэхүүн ангилал.
        /// </summary>
        [Required]
        public Category Category { get; set; } = null!;
    }
} 