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
        private Button btnSalesReport;
        private Panel productButtonPanel;
        private Panel categoryButtonPanel;

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
            InitializeCustomComponents();
            InitializeData();
        }

        private void InitializeCustomComponents()
        {
            // Initialize Product Tab
            InitializeProductTab();

            // Initialize Category Tab
            InitializeCategoryTab();

            // Initialize Reports Tab
            InitializeReportsTab();
        }

        private void InitializeProductTab()
        {
            // Create button panel
            productButtonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create and add buttons
            var btnAddProduct = new Button
            {
                Text = "Add Product",
                Location = new Point(10, 10),
                Size = new Size(100, 30)
            };
            btnAddProduct.Click += btnAddProduct_Click;

            var btnEditProduct = new Button
            {
                Text = "Edit Product",
                Location = new Point(120, 10),
                Size = new Size(100, 30)
            };
            btnEditProduct.Click += btnEditProduct_Click;

            var btnDeleteProduct = new Button
            {
                Text = "Delete Product",
                Location = new Point(230, 10),
                Size = new Size(100, 30)
            };
            btnDeleteProduct.Click += btnDeleteProduct_Click;

            productButtonPanel.Controls.AddRange(new Control[] { btnAddProduct, btnEditProduct, btnDeleteProduct });
            tabProducts.Controls.Add(productButtonPanel);

            // Add DataGridView
            if (productDataGridView != null)
            {
                productDataGridView.Parent = tabProducts;
                productDataGridView.Dock = DockStyle.Fill;
                productDataGridView.BringToFront();
            }
        }

        private void InitializeCategoryTab()
        {
            // Create button panel
            categoryButtonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create and add buttons
            var btnAddCategory = new Button
            {
                Text = "Add Category",
                Location = new Point(10, 10),
                Size = new Size(100, 30)
            };
            btnAddCategory.Click += btnAddCategory_Click;

            var btnEditCategory = new Button
            {
                Text = "Edit Category",
                Location = new Point(120, 10),
                Size = new Size(100, 30)
            };
            btnEditCategory.Click += btnEditCategory_Click;

            var btnDeleteCategory = new Button
            {
                Text = "Delete Category",
                Location = new Point(230, 10),
                Size = new Size(100, 30)
            };
            btnDeleteCategory.Click += btnDeleteCategory_Click;

            categoryButtonPanel.Controls.AddRange(new Control[] { btnAddCategory, btnEditCategory, btnDeleteCategory });
            tabCategories.Controls.Add(categoryButtonPanel);

            // Add DataGridView
            if (categoryDataGridView != null)
            {
                categoryDataGridView.Parent = tabCategories;
                categoryDataGridView.Dock = DockStyle.Fill;
                categoryDataGridView.BringToFront();
            }
        }

        private void InitializeReportsTab()
        {
            // Add Sales Report button to Reports tab
            btnSalesReport = new Button
            {
                Text = "View Sales Report",
                Location = new Point(20, 20),
                Size = new Size(150, 30)
            };
            btnSalesReport.Click += BtnSalesReport_Click;
            tabReports.Controls.Add(btnSalesReport);
        }

        private void BtnSalesReport_Click(object sender, EventArgs e)
        {
            using (var form = new SalesReportForm(_saleService, _receiptService))
            {
                form.ShowDialog();
            }
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