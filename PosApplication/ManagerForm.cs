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

namespace PosApplication
{
    public partial class ManagerForm : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly SaleService _saleService;
        private readonly ReceiptService _receiptService;

        public ManagerForm(UserService userService, ProductService productService, SaleService saleService, ReceiptService receiptService)
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PosDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
            _context = new ApplicationDbContext(options);
            _userService = userService;
            _productService = productService;
            _saleService = saleService;
            _receiptService = receiptService;
            InitializeData();
        }

        private void InitializeData()
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadProducts()
        {
            productDataGridView.DataSource = _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        private void LoadCategories()
        {
            categoryDataGridView.DataSource = _context.Categories.ToList();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var form = new ProductForm(_productService))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (productDataGridView.SelectedRows.Count > 0)
            {
                var product = (Product)productDataGridView.SelectedRows[0].DataBoundItem;
                using (var form = new ProductForm(_productService, product))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (productDataGridView.SelectedRows.Count > 0)
            {
                var product = (Product)productDataGridView.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show("Are you sure you want to delete this product?", "Confirmation",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    LoadProducts();
                }
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            using (var form = new CategoryForm(_productService))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                }
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (categoryDataGridView.SelectedRows.Count > 0)
            {
                var category = (Category)categoryDataGridView.SelectedRows[0].DataBoundItem;
                using (var form = new CategoryForm(_productService))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadCategories();
                    }
                }
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (categoryDataGridView.SelectedRows.Count > 0)
            {
                var category = (Category)categoryDataGridView.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show("Are you sure you want to delete this category?", "Confirmation",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                    LoadCategories();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _context.Dispose();
        }
    }
} 