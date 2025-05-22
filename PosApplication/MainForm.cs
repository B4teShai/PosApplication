using System;
using System.Windows.Forms;
using PosLibrary.Models;
using PosLibrary.Services;
using PosLibrary.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace PosApplication
{
    public partial class MainForm : Form
    {
        private readonly UserService _userService;
        private readonly ReceiptService _receiptService;
        private readonly PosLibrary.Models.UserRole _userRole;
        private User _currentUser;
        private decimal _currentTotal = 0;
        private int? _selectedCategoryId = null;
        private string currentCategory = "All";
        private List<PosLibrary.Models.Product> products = new List<PosLibrary.Models.Product>();
        private List<PosLibrary.Models.Product> filteredProducts = new List<PosLibrary.Models.Product>();
        private Cart _currentCart;
        private List<Category> categories = new List<Category>();
        private Dictionary<int, Button> categoryButtons = new Dictionary<int, Button>();
        private ProductService _productService;
        private CartService _cartService;
        private SaleService _saleService;
        private ApplicationDbContext _dbContext;

        public MainForm(User user)
        {
            InitializeComponent();
            
            // Одоогийн хэрэглэгчийн мэдээллийг хадгалах
            _currentUser = user;
            
            // Жагсаалтуудыг анхны тохиргоогоор үүсгэх
            categories = new List<Category>();
            categoryButtons = new Dictionary<int, Button>();
            
            SetupProductListSpacing();

            InitializeData();
            ConfigureUIForUserRole();
        }
        
        private void SetupProductListSpacing()
        {
            lvProducts.View = View.LargeIcon;
            lvProducts.Padding = new Padding(10);
            
            // Зургийн жагсаалтыг тохируулах
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(160, 160);
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            lvProducts.LargeImageList = imgList;
        }
            
        private async void InitializeData()
        {
            try
            {
                // Өгөгдлийн сангийн тохиргоо
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite("Data Source=pos.db")
                    .Options;
                
                _dbContext = new ApplicationDbContext(options);
                
                _productService = new ProductService(_dbContext);
                _cartService = new CartService();
                _saleService = new SaleService(_dbContext);
                
                categoryButtons = new Dictionary<int, Button>();
                categoryButtons[0] = btnAllCategories;
                
                await LoadCategories();

                LoadProducts();
                
                _cartService.ClearCart();
                UpdateCartDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Өгөгдлийн тохиргоонд алдаа гарлаа: {ex.Message}", "Алдаа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDefaultUser()
        {
            try
            {
                // Эрхэнд тохируулан анхны хэрэглэгчийн мэдээллийг үүсгэх
                switch (_userRole)
                {
                    case PosLibrary.Models.UserRole.Manager:
                        _currentUser = new Manager { Username = "manager", FullName = "Системийн удирдагч" };
                        break;
                    case PosLibrary.Models.UserRole.Cashier1:
                        _currentUser = new Cashier { Username = "cashier1", FullName = "Кассчин 1" };
                        break;
                    case PosLibrary.Models.UserRole.Cashier2:
                        _currentUser = new Cashier { Username = "cashier2", FullName = "Кассчин 2" };
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Хэрэглэгчийн мэдээлэл тохируулахад алдаа гарлаа: {ex.Message}",
                    "Нэвтрэх алдаа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupRoleBasedAccess()
        {
            this.Text = $"POS Систем - {_userRole}";
            if (_userRole == PosLibrary.Models.UserRole.Cashier1 || _userRole == PosLibrary.Models.UserRole.Cashier2)
            {
                menuCategory.Visible = false;
            }

            if (_userRole == PosLibrary.Models.UserRole.Manager)
            {
                lvProducts.MouseClick += LvProducts_MouseClick;
            }
        }

        private async void LoadProducts(int? categoryId = null, string searchTerm = null)
        {
            try
            {
                int? filterCategoryId = categoryId ?? _selectedCategoryId;
                
                lvProducts.Items.Clear();
                
                var productsTask = filterCategoryId.HasValue 
                    ? _productService.GetProductsByCategoryAsync(filterCategoryId.Value) 
                    : _productService.GetAllProducts();
                
                var products = productsTask.GetAwaiter().GetResult();
                
                // Хайлт
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(p => 
                        p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                
                // ListView-г дэлгэрэнгүй харуулах
                if (lvProducts.LargeImageList == null)
                {
                    lvProducts.LargeImageList = new ImageList();
                    lvProducts.LargeImageList.ImageSize = new Size(160, 160);
                    lvProducts.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
                }
                else
                {
                    lvProducts.LargeImageList.Images.Clear();
                }
                
                // Бүтээгдэхүүнүүдийг ListView-д нэмэх
                foreach (var product in products)
                {
                    Bitmap productImage;
                    
                    if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
                    {
                        try 
                        {
                            using (var img = Image.FromFile(product.ImagePath))
                            {
                                productImage = new Bitmap(img);
                            }
                        }
                        catch
                        {
                            productImage = CreateProductImage(product);
                        }
                    }
                    else
                    {
                        productImage = CreateProductImage(product);
                    }
                    
                    lvProducts.LargeImageList.Images.Add(product.Id.ToString(), productImage);
                    
                    var item = new ListViewItem();
                    item.Text = product.Name; 
                    item.ImageKey = product.Id.ToString();
                    item.Tag = product;
                    item.ToolTipText = $"Нэр: {product.Name}\nҮнэ: ${product.Price:F2}\nҮлдэгдэл: {product.StockQuantity}";
                    
                    lvProducts.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Бүтээгдэхүүнүүдийг ачаалахад алдаа гарлаа: {ex.Message}", "Алдаа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap CreateProductImage(Product product)
        {
            Bitmap bmp = new Bitmap(160, 160);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    Color.FromArgb(250, 250, 255),
                    Color.FromArgb(235, 235, 245),
                    45f))
                {
                    g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                }
                
                Rectangle borderRect = new Rectangle(1, 1, bmp.Width - 3, bmp.Height - 3);
                using (GraphicsPath borderPath = CreateRoundedRectangle(borderRect, 8))
                using (Pen pen = new Pen(Color.FromArgb(180, 180, 220), 2))
                {
                    g.DrawPath(pen, borderPath);
                }
                
                Rectangle shadowRect = new Rectangle(3, 3, bmp.Width - 7, bmp.Height - 7);
                using (GraphicsPath shadowPath = CreateRoundedRectangle(shadowRect, 7))
                using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                {
                    shadowBrush.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    shadowBrush.SurroundColors = new Color[] { Color.FromArgb(20, 0, 0, 0) };
                    g.FillPath(shadowBrush, shadowPath);
                }
                
                using (Font codeFont = new Font("Segoe UI", 9, FontStyle.Italic))
                using (SolidBrush codeBrush = new SolidBrush(Color.FromArgb(130, 130, 150)))
                {
                    g.DrawString(product.Code, codeFont, codeBrush, new PointF(10, 10));
                }
                
                Rectangle nameBox = new Rectangle(10, 28, bmp.Width - 20, 48);
                using (GraphicsPath namePath = CreateRoundedRectangle(nameBox, 6))
                using (LinearGradientBrush nameBgBrush = new LinearGradientBrush(
                    nameBox,
                    Color.FromArgb(120, 160, 220),
                    Color.FromArgb(90, 130, 200),
                    90f))
                {
                    g.FillPath(nameBgBrush, namePath);
                    
                    using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                        new Rectangle(nameBox.X, nameBox.Y, nameBox.Width, 6),
                        Color.FromArgb(60, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        90f))
                    {
                        g.FillRectangle(highlightBrush, nameBox.X + 2, nameBox.Y + 1, nameBox.Width - 4, 5);
                    }
                }
                
                using (Font nameFont = new Font("Segoe UI", 12, FontStyle.Bold))
                {
                    StringFormat nameFormat = new StringFormat();
                    nameFormat.Alignment = StringAlignment.Center;
                    nameFormat.LineAlignment = StringAlignment.Center;
                    nameFormat.Trimming = StringTrimming.EllipsisCharacter;
                    
                    // Draw shadow
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                    {
                        g.DrawString(product.Name, nameFont, shadowBrush, new RectangleF(nameBox.X + 2, nameBox.Y + 2, nameBox.Width, nameBox.Height), nameFormat);
                    }
                    
                    // Draw text
                    using (SolidBrush nameBrush = new SolidBrush(Color.White))
                    {
                        g.DrawString(product.Name, nameFont, nameBrush, nameBox, nameFormat);
                    }
                }

                Rectangle priceBox = new Rectangle(20, 90, bmp.Width - 40, 30);
                using (GraphicsPath pricePath = CreateRoundedRectangle(priceBox, 5))
                using (LinearGradientBrush priceBgBrush = new LinearGradientBrush(
                    priceBox,
                    Color.FromArgb(245, 245, 245),
                    Color.FromArgb(230, 230, 235),
                    90f))
                {
                    g.FillPath(priceBgBrush, pricePath);
                    
                    using (Pen priceBorderPen = new Pen(Color.FromArgb(190, 190, 200), 1))
                    {
                        g.DrawPath(priceBorderPen, pricePath);
                    }
                }
                
                using (Font priceFont = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush priceBrush = new SolidBrush(Color.FromArgb(20, 110, 20)))
                {
                    StringFormat priceFormat = new StringFormat();
                    priceFormat.Alignment = StringAlignment.Center;
                    priceFormat.LineAlignment = StringAlignment.Center;
                    
                    g.DrawString($"${product.Price:F2}", priceFont, priceBrush, priceBox, priceFormat);
                }
                
                int stockIndicatorSize = 12;
                Color stockColor = product.StockQuantity > 10 ? Color.FromArgb(0, 160, 0) : 
                                  product.StockQuantity > 0 ? Color.FromArgb(240, 150, 0) : 
                                  Color.FromArgb(200, 40, 40);
                
                Rectangle stockIndicatorRect = new Rectangle(15, 130, stockIndicatorSize, stockIndicatorSize);
                using (GraphicsPath stockPath = new GraphicsPath())
                {
                    stockPath.AddEllipse(stockIndicatorRect);
                    
                    using (PathGradientBrush stockBrush = new PathGradientBrush(stockPath))
                    {
                        Color centerColor = Color.FromArgb(255, stockColor);
                        Color[] surroundColors = { Color.FromArgb(200, stockColor) };
                        
                        stockBrush.CenterColor = centerColor;
                        stockBrush.SurroundColors = surroundColors;
                        
                        g.FillPath(stockBrush, stockPath);
                    }
                    
                    using (GraphicsPath highlightPath = new GraphicsPath())
                    {
                        highlightPath.AddEllipse(new Rectangle(stockIndicatorRect.X + 2, stockIndicatorRect.Y + 2, 
                                                stockIndicatorRect.Width / 2, stockIndicatorRect.Height / 2));
                        using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(120, 255, 255, 255)))
                        {
                            g.FillPath(highlightBrush, highlightPath);
                        }
                    }
                }
                
                using (Font stockFont = new Font("Segoe UI", 9))
                using (SolidBrush stockTextBrush = new SolidBrush(Color.FromArgb(70, 70, 80)))
                {
                    string stockText = product.StockQuantity > 10 ? $"In Stock ({product.StockQuantity})" : 
                                      product.StockQuantity > 0 ? $"Low Stock ({product.StockQuantity})" : 
                                      "Out of Stock";
                    g.DrawString(stockText, stockFont, stockTextBrush, new PointF(30, 130));
                }
            }
            
            return bmp;
        }

        private void LvProducts_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
            
            if (e.Item.Tag is Product product)
            {
                const int margin = 5;
                
                int imageHeight = lvProducts.LargeImageList.ImageSize.Height;
                var imageRect = new Rectangle(
                    e.Bounds.X + margin,
                    e.Bounds.Y + margin,
                    e.Bounds.Width - (margin * 2),
                    imageHeight
                );
                
                if ((e.State & ListViewItemStates.Selected) != 0)
                {
                    using (var path = CreateRoundedRectangle(imageRect, 8))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        
                        using (var brush = new LinearGradientBrush(
                            imageRect,
                            Color.FromArgb(100, 220, 230, 250),
                            Color.FromArgb(100, 180, 200, 240),
                            45F))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        
                        using (var pen = new Pen(Color.FromArgb(150, 100, 150, 200), 2))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
                else
                {
                    using (var path = CreateRoundedRectangle(imageRect, 8))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        
                        using (var brush = new LinearGradientBrush(
                            imageRect,
                            Color.FromArgb(80, 250, 250, 255),
                            Color.FromArgb(80, 240, 240, 250),
                            45F))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        
                        using (var pen = new Pen(Color.FromArgb(120, 220, 220, 240), 1))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
                
                if (e.Item.ImageKey != null && lvProducts.LargeImageList != null)
                {
                    Image img = lvProducts.LargeImageList.Images[e.Item.ImageKey];
                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, imageRect);
                    }
                }
                
                string itemText = product.Name;
                Font textFont = e.Item.Font ?? lvProducts.Font;
                Brush textBrush = (e.State & ListViewItemStates.Selected) != 0 
                    ? new SolidBrush(Color.FromArgb(0, 70, 140))
                    : new SolidBrush(Color.FromArgb(60, 60, 80));
                
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                
                var textRect = new Rectangle(
                    e.Bounds.X,
                    e.Bounds.Y + imageHeight + margin * 2,
                    e.Bounds.Width,
                    e.Bounds.Height - imageHeight - margin * 2
                );
                
                // e.Graphics.DrawString(itemText, textFont, textBrush, textRect, sf);
            }
        }

        private void LvProducts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void LvProducts_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Already logged in automatically.", "Login Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddQuantityControls(PosLibrary.Models.Product product)
        {
            var panel = new Panel
            {
                Size = new Size(300, 50),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblProductName = new Label
            {
                Text = product.Name,
                Location = new Point(10, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblProductName);

            var lblQuantity = new Label
            {
                Text = "1",
                Location = new Point(200, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblQuantity);

            var btnIncrease = new Button
            {
                Text = "+",
                Location = new Point(250, 10),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat
            };
            btnIncrease.FlatAppearance.BorderSize = 0;
            btnIncrease.BackColor = Color.White;
            btnIncrease.Paint += (s, e) =>
            {
                var button = (Button)s;
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, button.Width, button.Height);
                button.Region = new Region(path);
            };
            btnIncrease.Click += (s, e) =>
            {
                int quantity = int.Parse(lblQuantity.Text);
                lblQuantity.Text = (++quantity).ToString();
            };
            
            panel.Controls.Add(btnIncrease);

            var btnDecrease = new Button
            {
                Text = "-",
                Location = new Point(160, 10),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat
            };
            btnDecrease.FlatAppearance.BorderSize = 0;
            btnDecrease.BackColor = Color.White;
            btnDecrease.Paint += (s, e) =>
            {
                var button = (Button)s;
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, button.Width, button.Height);
                button.Region = new Region(path);
            };
            btnDecrease.Click += (s, e) =>
            {
                int quantity = int.Parse(lblQuantity.Text);
                if(quantity > 1)
                {
                    lblQuantity.Text = (--quantity).ToString();
                }
            };
            
            panel.Controls.Add(btnDecrease);
        }

        private void BtnAddToSale_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvProducts.SelectedItems.Count == 0)
                    return;
                
                var selectedItem = lvProducts.SelectedItems[0];
                var product = (PosLibrary.Models.Product)selectedItem.Tag;
                
                if (product == null)
                    return;
                
                // Add product to cart
                AddProductToCart(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to sale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnCompleteSale_Click(object sender, EventArgs e)
        {
            var cart = _cartService.GetCart();
            
            if (cart == null || cart.Items.Count == 0)
            {
                MessageBox.Show("Please add items to the cart before completing the sale.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Show payment form
                var paymentForm = new PaymentForm(cart);
                var result = paymentForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Create sale record
                    var sale = new Sale
                    {
                        UserId = _currentUser.Id,
                        Total = cart.Total,
                        AmountPaid = cart.AmountPaid,
                        Change = cart.AmountPaid - cart.Total,
                        CreatedAt = DateTime.Now,
                        Items = cart.Items.Select(i => new SaleItem
                        {
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            Subtotal = i.Subtotal
                        }).ToList()
                    };

                    this.Cursor = Cursors.WaitCursor;
                    
                    await _saleService.CreateSale(sale);
                    
                    await LoadCategories();
                    LoadProducts();

                    _cartService.ClearCart();
                    UpdateCartDisplay();
                    UpdateTotal();
                    
                    this.Cursor = Cursors.Default;

                    MessageBox.Show("Sale completed successfully! Stock quantities have been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Error completing sale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadCategories()
        {
            try
            {
                categories = await _productService.GetCategoriesAsync();
                
                foreach (var kvp in categoryButtons)
                {
                    if (kvp.Key != 0)
                    {
                        pnlCategoryFilter.Controls.Remove(kvp.Value);
                        kvp.Value.Dispose();
                    }
                }
                
                categoryButtons.Clear();
                
                categoryButtons[0] = btnAllCategories;
                
                int xPosition = 70;
                
                foreach (var category in categories)
                {
                    Button categoryButton = new Button();
                    categoryButton.Text = category.Name;
                    categoryButton.Location = new Point(xPosition, 3);
                    categoryButton.Size = new Size(70, 24);
                    categoryButton.FlatStyle = FlatStyle.Flat;
                    categoryButton.BackColor = Color.FromArgb(240, 240, 240);
                    categoryButton.Tag = category.Id;
                    categoryButton.Click += CategoryButton_Click;
                    
                    pnlCategoryFilter.Controls.Add(categoryButton);
                    categoryButtons[category.Id] = categoryButton;
                    
                    xPosition += 75;
                }
                
                UpdateCategoryButtonStyles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int categoryId = (int)clickedButton.Tag;
            
            _selectedCategoryId = categoryId;
            
            UpdateCategoryButtonStyles();
            
            LoadProducts(categoryId: categoryId);
        }

        private void UpdateCategoryButtonStyles()
        {
            btnAllCategories.BackColor = Color.FromArgb(240, 240, 240);
            btnAllCategories.ForeColor = Color.Black;
            
            foreach (var button in categoryButtons.Values)
            {
                button.BackColor = Color.FromArgb(240, 240, 240);
                button.ForeColor = Color.Black;
            }

            if (_selectedCategoryId.HasValue)
            {
                categoryButtons[_selectedCategoryId.Value].BackColor = Color.FromArgb(100, 150, 200);
                categoryButtons[_selectedCategoryId.Value].ForeColor = Color.White;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts(searchTerm: txtSearch.Text);
        }

        private void LvProducts_MouseClick(object sender, MouseEventArgs e)
        {
            if (lvProducts.SelectedItems.Count == 0)
                return;
        
            var hitItem = lvProducts.GetItemAt(e.X, e.Y);
            if (hitItem == null)
                return;
        
            var product = (PosLibrary.Models.Product)hitItem.Tag;
            
            if (e.Button == MouseButtons.Left)
            {
                AddProductToCart(product);
            }
            else if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                
                if (_currentUser.CanEditProducts())
                {
                    var editItem = contextMenu.Items.Add("Edit Product");
                    editItem.Image = CreateMenuIcon(Color.FromArgb(100, 150, 200));
                    editItem.Click += (s, args) => EditProduct(product);
                    
                    if (_currentUser.CanDeleteProducts())
                    {
                        var deleteItem = contextMenu.Items.Add("Delete Product");
                        deleteItem.Image = CreateMenuIcon(Color.FromArgb(220, 80, 80));
                        deleteItem.Click += (s, args) => DeleteProduct(product);
                    }
                    
                    contextMenu.Show(lvProducts, e.Location);
                }
            }
        }

        private Bitmap CreateMenuIcon(Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, 0, 0, 16, 16);
                }
            }
            return bmp;
        }

        private void DisplayProductPanel(PosLibrary.Models.Product product)
        {
            var panel = new Panel
            {
                Size = new Size(300, 200),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblName = new Label
            {
                Text = product.Name,
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold)
            };
            panel.Controls.Add(lblName);

            var lblPrice = new Label
            {
                Text = $"Price: ${product.Price:F2}",
                Location = new Point(10, 40),
                AutoSize = true
            };
            panel.Controls.Add(lblPrice);

            var lblStock = new Label
            {
                Text = product.StockQuantity > 0 ? 
                      $"Stock: {product.StockQuantity} (can order more if needed)" : 
                      "Special order item",
                Location = new Point(10, 70),
                AutoSize = true
            };
            panel.Controls.Add(lblStock);

            var lblDescription = new Label
            {
                Text = product.Description,
                Location = new Point(10, 100),
                AutoSize = true,
                MaximumSize = new Size(280, 0)
            };
            panel.Controls.Add(lblDescription);

            var btnAddToCart = new Button
            {
                Text = "Add to Cart",
                Location = new Point(10, 150),
                Size = new Size(280, 30)
            };
            btnAddToCart.Click += (s, e) =>
            {
                if (_currentCart == null)
                {
                    _currentCart = new Cart { UserId = _currentUser.Id };
                }

                var existingItem = _currentCart.Items.FirstOrDefault(i => i.ProductId == product.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    _currentCart.Items.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = 1,
                        UnitPrice = product.Price
                    });
                }

                UpdateTotal();
                panel.Dispose();
            };
            panel.Controls.Add(btnAddToCart);
        }

        private void UpdateTotalPrice(decimal priceChange)
        {
            _currentTotal += priceChange;
            lblTotal.Text = $"Total: {_currentTotal:C}";
        }

        private async void EditProduct(PosLibrary.Models.Product product)
        {
            if (product == null)
                return;
        
            try
            {
                var productForm = new ProductForm(_productService, product);
                
                if (productForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadCategories();
                    LoadProducts();
                    MessageBox.Show($"Product '{product.Name}' updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteProduct(PosLibrary.Models.Product product)
        {
            if (product == null)
                return;
        
            var result = MessageBox.Show(
                $"Are you sure you want to delete the following product?\n\n" +
                $"Name: {product.Name}\n" +
                $"Code: {product.Code}\n" +
                $"Price: ${product.Price:F2}\n" +
                $"Stock: {product.StockQuantity}\n\n" +
                "This action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
        
            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    
                    bool success = await _productService.DeleteProduct(product.Id);
                    
                    Cursor = Cursors.Default;
                    
                    if (success)
                    {
                        await LoadCategories();
                        LoadProducts();
                        MessageBox.Show($"Product '{product.Name}' deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product. It may be referenced by sales records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show($"Error deleting product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void MenuProduct_Click(object sender, EventArgs e)
        {
            if (_currentUser.Role == PosLibrary.Models.UserRole.Manager)
            {
                var productForm = new ProductForm(_productService);
                if (productForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadCategories();
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to manage products.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void MenuCategory_Click(object sender, EventArgs e)
        {
            if (_currentUser.Role == PosLibrary.Models.UserRole.Manager)
            {
                var categoryForm = new CategoryForm(_productService);
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadCategories();
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to manage categories.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MenuHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "POS System Help\n\n" +
                "1. Use the search box to find products\n" +
                "2. Click on a product to add it to cart\n" +
                "3. Use + and - buttons to adjust quantities\n" +
                "4. Click 'Complete Sale' to process payment\n\n" +
                "For managers:\n" +
                "- Right-click products to edit or delete\n" +
                "- Use the menu to manage items and categories",
                "Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        
        private void MenuLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void UpdateProductList()
        {
            try
            {
                lvProducts.Items.Clear();
                foreach (var product in filteredProducts)
                {
                    var item = new ListViewItem();
                    item.Text = product.Name;
                    item.Tag = product;
                    lvProducts.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                filteredProducts = products.Where(p =>
                    (string.IsNullOrEmpty(txtSearch.Text) ||
                     p.Name.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                     p.Description.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)) &&
                    (!_selectedCategoryId.HasValue || p.CategoryId == _selectedCategoryId.Value))
                    .ToList();

                UpdateProductList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotal()
        {
            var cart = _cartService.GetCart();
            _currentTotal = cart.Total;
            lblTotal.Text = $"Total: ${_currentTotal:F2}";
            
            btnCompleteSale.Enabled = _currentTotal > 0;
            btnCompleteSale.BackColor = _currentTotal > 0
                ? Color.FromArgb(100, 150, 200)
                : Color.FromArgb(200, 200, 200);
        }

        private void TxtGoodsCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                AddProductByCode();
            }
        }

        private void BtnAddByCode_Click(object sender, EventArgs e)
        {
            AddProductByCode();
        }

        private async void AddProductByCode()
        {
            string code = txtGoodsCode.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please enter a goods code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var product = await _productService.GetProductByCode(code);
                if (product != null)
                {
                    AddProductToCart(product);
                    txtGoodsCode.Clear();
                    txtGoodsCode.Focus();
                }
                else
                {
                    MessageBox.Show("Product not found with the specified code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProductToCart(PosLibrary.Models.Product product)
        {
            if (product == null)
                return;
        
            try
            {
                _cartService.AddToCart(product);
                
                var cartItem = _cartService.GetCart().Items.Find(i => i.ProductId == product.Id);
                if (cartItem != null && cartItem.Quantity > product.StockQuantity && product.StockQuantity > 0)
                {
                    MessageBox.Show(
                        $"You've ordered {cartItem.Quantity} units of {product.Name}, but only {product.StockQuantity} are in stock.\n\n" +
                        "Your order can still be processed, but may require additional time for the extra items.",
                        "Stock Notice",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                
                UpdateCartDisplay();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product to cart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAllCategories_Click(object sender, EventArgs e)
        {
            _selectedCategoryId = null;
                
            UpdateCategoryButtonStyles();
            LoadProducts();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            _dbContext?.Dispose();
        }

        private void ConfigureUIForUserRole()
        {
            if (_currentUser.Role == PosLibrary.Models.UserRole.Manager)
            {
                menuCategory.Visible = true;
                menuProduct.Visible = true;
            }
            else
            {
                menuCategory.Visible = false;
                menuProduct.Visible = false;
            }
        }

        private void UpdateCartDisplay()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                
                var cart = _cartService.GetCart();
                
                foreach (var item in cart.Items)
                {
                    var panel = new Panel
                    {
                        Size = new Size(580, 60),
                        Margin = new Padding(0, 0, 0, 10),
                        Tag = item
                    };
                    
                    panel.Paint += (s, e) => 
                    {
                        var p = (Panel)s;
                        var g = e.Graphics;

                        var rect = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                        using (var path = CreateRoundedRectangle(rect, 8))
                        using (var brush = new LinearGradientBrush(
                            rect, 
                            Color.FromArgb(250, 250, 255), 
                            Color.FromArgb(240, 240, 250), 
                            LinearGradientMode.Vertical))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.FillPath(brush, path);
                            
                            using (var pen = new Pen(Color.FromArgb(200, 200, 230), 1))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                    };
                    
                    var lblName = new Label
                    {
                        Text = item.ProductName,
                        Location = new Point(15, 10),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(50, 50, 100),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblName);
                    
                    var lblQuantityTitle = new Label
                    {
                        Text = "Qty:",
                        Location = new Point(15, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9),
                        ForeColor = Color.FromArgb(100, 100, 130),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblQuantityTitle);
                    
                    var lblQuantity = new Label
                    {
                        Name = "lblQuantity",
                        Text = item.Quantity.ToString(),
                        Location = new Point(45, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(80, 80, 120),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblQuantity);
                    
                    var lblPriceTitle = new Label
                    {
                        Text = "Price:",
                        Location = new Point(100, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9),
                        ForeColor = Color.FromArgb(100, 100, 130),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblPriceTitle);
                    
                    var lblPrice = new Label
                    {
                        Text = $"${item.UnitPrice:F2}",
                        Location = new Point(140, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 100, 0),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblPrice);
                    
                    var lblSubtotalTitle = new Label
                    {
                        Text = "Subtotal:",
                        Location = new Point(220, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9),
                        ForeColor = Color.FromArgb(100, 100, 130),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblSubtotalTitle);
                    
                    var lblSubtotal = new Label
                    {
                        Text = $"${item.Subtotal:F2}",
                        Location = new Point(280, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 100, 0),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(lblSubtotal);
                    
                    var btnDecrease = new Button
                    {
                        Text = "-",
                        Location = new Point(400, 15),
                        Size = new Size(30, 30),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.FromArgb(100, 100, 150),
                        Tag = item.ProductId
                    };
                    btnDecrease.FlatAppearance.BorderSize = 0;
                    btnDecrease.Click += (s, e) => 
                    {
                        int productId = (int)((Button)s).Tag;
                        var cartItem = _cartService.GetCart().Items.Find(i => i.ProductId == productId);
                        if (cartItem != null && cartItem.Quantity > 1)
                        {
                            _cartService.UpdateQuantity(productId, cartItem.Quantity - 1);
                            UpdateCartDisplay();
                            UpdateTotal();
                        }
                    };
                    panel.Controls.Add(btnDecrease);
                    
                    var btnIncrease = new Button
                    {
                        Text = "+",
                        Location = new Point(440, 15),
                        Size = new Size(30, 30),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.FromArgb(100, 100, 150),
                        Tag = item.ProductId
                    };
                    btnIncrease.FlatAppearance.BorderSize = 0;
                    btnIncrease.Click += (s, e) => 
                    {
                        int productId = (int)((Button)s).Tag;
                        var cartItem = _cartService.GetCart().Items.Find(i => i.ProductId == productId);
                        if (cartItem != null)
                        {
                            var product = _productService.GetProductById(productId).GetAwaiter().GetResult();
                            
                            _cartService.UpdateQuantity(productId, cartItem.Quantity + 1);
                            
                            if (product != null && cartItem.Quantity + 1 > product.StockQuantity && product.StockQuantity > 0 
                                && cartItem.Quantity == product.StockQuantity + 1)
                            {
                                MessageBox.Show(
                                    $"You've ordered {cartItem.Quantity + 1} units of {product.Name}, but only {product.StockQuantity} are in stock.\n\n" +
                                    "Your order can still be processed, but may require additional time for the extra items.",
                                    "Stock Notice",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                            
                            UpdateCartDisplay();
                            UpdateTotal();
                        }
                    };
                    panel.Controls.Add(btnIncrease);
                    
                    var btnRemove = new Button
                    {
                        Text = "×",
                        Location = new Point(530, 15),
                        Size = new Size(30, 30),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.FromArgb(200, 60, 60),
                        Tag = item.ProductId
                    };
                    btnRemove.FlatAppearance.BorderSize = 0;
                    btnRemove.Click += (s, e) => 
                    {
                        int productId = (int)((Button)s).Tag;
                        _cartService.RemoveFromCart(productId);
                        UpdateCartDisplay();
                        UpdateTotal();
                    };
                    panel.Controls.Add(btnRemove);
                    
                    flowLayoutPanel1.Controls.Add(panel);
                }
                
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating cart display: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
} 