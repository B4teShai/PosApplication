using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosLibrary.Models
{
    /// <summary>
    /// POS системд хэрэглэгчдийн үүсгэнэ.
    /// </summary>
    public enum UserRole
    {
        Manager,
        Cashier1,
        Cashier2
    }

    public abstract class User
    {
        /// <summary>
        /// Хэрэглэгчийн ID.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Хэрэглэгчийн нэр.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// Хэрэглэгчийн нууц үг.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// Хэрэглэгчийн эрхийг илэрхийлнэ.
        /// </summary>
        [Required]
        public UserRole Role { get; set; }

        /// <summary>
        /// Хэрэглэгч бүтээгдэхүүн өөрчилж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public abstract bool CanEditProducts();

        /// <summary>
        /// Хэрэглэгч бүтээгдэхүүн устгаж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public abstract bool CanDeleteProducts();

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }

    /// <summary>
    /// Менежер хэрэглэгч бүтээгдэхүүн үйлдэл хийх чадвартай.
    /// </summary>
    public class Manager : User
    {
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Менежер хэрэглэгч классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        public Manager()
        {
            Role = UserRole.Manager;
        }

        /// <summary>
        /// Менежер бүтээгдэхүүн өөрчилж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public override bool CanEditProducts()
        {
            return true;
        }

        /// <summary>
        /// Менежер бүтээгдэхүүн устгаж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public override bool CanDeleteProducts()
        {
            return true;
        }
    }

    /// <summary>
    /// Кассир хэрэглэгч бүтээгдэхүүн үйлдэл хийх чадвартай.
    /// </summary>
    public class Cashier : User
    {
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Кассир хэрэглэгч классын шинэ жишээг эхлүүлнэ.
        /// </summary>
        public Cashier()
        {
            Role = UserRole.Cashier1;
        }

        /// <summary>
        /// Кассир бүтээгдэхүүн өөрчилж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public override bool CanEditProducts()
        {
            return false;
        }

        /// <summary>
        /// Кассир бүтээгдэхүүн устгаж чадах эсэхийг илэрхийлнэ.
        /// </summary>
        /// <returns>Үнэн бол True; Үгүй бол False.</returns>
        public override bool CanDeleteProducts()
        {
            return false;
        }
    }
} 