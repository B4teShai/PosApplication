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
            
            // Store the current user
            _currentUser = user;
            
            // Initialize collections
            categories = new List<Category>();
            categoryButtons = new Dictionary<int, Button>();
            
            // Set consistent spacing for products
            SetupProductListSpacing();
            
            // Initialize data
            InitializeData();
            
            // Set up UI based on user role
            ConfigureUIForUserRole();
        }
        
        private void SetupProductListSpacing()
        {
            // Configure ListView for proper spacing
            lvProducts.View = View.LargeIcon;
            lvProducts.Padding = new Padding(10);
            
            // Make sure items are properly spaced in the grid
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(160, 160);
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            lvProducts.LargeImageList = imgList;
        }
            
        private async void InitializeData()
        {
            try
            {
                // Create a properly configured DbContext
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite("Data Source=pos.db")
                    .Options;
                
                _dbContext = new ApplicationDbContext(options);
                
                // Initialize services with the same context instance
                _productService = new ProductService(_dbContext);
                _cartService = new CartService();
                _saleService = new SaleService(_dbContext);
                
                // Initialize category buttons dictionary
                categoryButtons = new Dictionary<int, Button>();
                categoryButtons[0] = btnAllCategories; // Add "All" button with ID 0
                
                // Load categories first
                await LoadCategories();
                
                // Then load products
                LoadProducts();
                
                // Initialize cart
                _cartService.ClearCart();
                UpdateCartDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDefaultUser()
        {
            try
            {
                // Create default user based on role
                switch (_userRole)
                {
                    case PosLibrary.Models.UserRole.Manager:
                        _currentUser = new Manager { Username = "manager", FullName = "System Manager" };
                        break;
                    case PosLibrary.Models.UserRole.Cashier1:
                        _currentUser = new Cashier { Username = "cashier1", FullName = "Cashier 1" };
                        break;
                    case PosLibrary.Models.UserRole.Cashier2:
                        _currentUser = new Cashier { Username = "cashier2", FullName = "Cashier 2" };
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting user: {ex.Message}",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupRoleBasedAccess()
        {
            this.Text = $"POS System - {_userRole}";

            // Configure menu access based on role
            if (_userRole == PosLibrary.Models.UserRole.Cashier1 || _userRole == PosLibrary.Models.UserRole.Cashier2)
            {
                menuCategory.Visible = false;
            }

            // Product list view - only managers can edit/delete products
            if (_userRole == PosLibrary.Models.UserRole.Manager)
            {
                lvProducts.MouseClick += LvProducts_MouseClick;
            }
        }

        private async void LoadProducts(int? categoryId = null, string searchTerm = null)
        {
            try
            {
                // Use the provided categoryId or the currently selected category
                int? filterCategoryId = categoryId ?? _selectedCategoryId;
                
                // Clear existing items
                lvProducts.Items.Clear();
                
                // Get products (filtered by category if specified)
                var productsTask = filterCategoryId.HasValue 
                    ? _productService.GetProductsByCategoryAsync(filterCategoryId.Value) 
                    : _productService.GetAllProducts();
                
                var products = productsTask.GetAwaiter().GetResult();
                
                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(p => 
                        p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                
                // Set up ListView for better display
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
                
                // Add products to ListView with improved visual style
                foreach (var product in products)
                {
                    // Load product image if available
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
                            // If image loading fails, create default image
                            productImage = CreateProductImage(product);
                        }
                    }
                    else
                    {
                        // Create a placeholder image for the product
                        productImage = CreateProductImage(product);
                    }
                    
                    // Add image to the image list
                    lvProducts.LargeImageList.Images.Add(product.Id.ToString(), productImage);
                    
                    // Create ListView item with the image
                    var item = new ListViewItem();
                    item.Text = product.Name; // Show product name below the image
                    item.ImageKey = product.Id.ToString();
                    item.Tag = product;
                    item.ToolTipText = $"Name: {product.Name}\nPrice: ${product.Price:F2}\nStock: {product.StockQuantity}";
                    
                    lvProducts.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap CreateProductImage(Product product)
        {
            // Create a bitmap for the product with larger dimensions for better visibility
            Bitmap bmp = new Bitmap(160, 160);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Enable high quality rendering
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                
                // Fill background with a more appealing gradient
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    Color.FromArgb(250, 250, 255),
                    Color.FromArgb(235, 235, 245),
                    45f))
                {
                    g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                }
                
                // Draw a more elegant border with rounded corners
                Rectangle borderRect = new Rectangle(1, 1, bmp.Width - 3, bmp.Height - 3);
                using (GraphicsPath borderPath = CreateRoundedRectangle(borderRect, 8))
                using (Pen pen = new Pen(Color.FromArgb(180, 180, 220), 2))
                {
                    g.DrawPath(pen, borderPath);
                }
                
                // Add subtle inner shadow effect
                Rectangle shadowRect = new Rectangle(3, 3, bmp.Width - 7, bmp.Height - 7);
                using (GraphicsPath shadowPath = CreateRoundedRectangle(shadowRect, 7))
                using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                {
                    shadowBrush.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    shadowBrush.SurroundColors = new Color[] { Color.FromArgb(20, 0, 0, 0) };
                    g.FillPath(shadowBrush, shadowPath);
                }
                
                // Draw product code with better styling
                using (Font codeFont = new Font("Segoe UI", 9, FontStyle.Italic))
                using (SolidBrush codeBrush = new SolidBrush(Color.FromArgb(130, 130, 150)))
                {
                    g.DrawString(product.Code, codeFont, codeBrush, new PointF(10, 10));
                }
                
                // Draw name box background with improved gradient and larger area
                Rectangle nameBox = new Rectangle(10, 28, bmp.Width - 20, 48);
                using (GraphicsPath namePath = CreateRoundedRectangle(nameBox, 6))
                using (LinearGradientBrush nameBgBrush = new LinearGradientBrush(
                    nameBox,
                    Color.FromArgb(120, 160, 220),
                    Color.FromArgb(90, 130, 200),
                    90f))
                {
                    g.FillPath(nameBgBrush, namePath);
                    
                    // Add subtle highlight to the top of the name box
                    using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                        new Rectangle(nameBox.X, nameBox.Y, nameBox.Width, 6),
                        Color.FromArgb(60, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        90f))
                    {
                        g.FillRectangle(highlightBrush, nameBox.X + 2, nameBox.Y + 1, nameBox.Width - 4, 5);
                    }
                }
                
                // Draw product name with better shadow and text handling
                using (Font nameFont = new Font("Segoe UI", 12, FontStyle.Bold))
                {
                    // Create a StringFormat object for center alignment
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
                
                // Price box with rounded corners and gradient
                Rectangle priceBox = new Rectangle(20, 90, bmp.Width - 40, 30);
                using (GraphicsPath pricePath = CreateRoundedRectangle(priceBox, 5))
                using (LinearGradientBrush priceBgBrush = new LinearGradientBrush(
                    priceBox,
                    Color.FromArgb(245, 245, 245),
                    Color.FromArgb(230, 230, 235),
                    90f))
                {
                    g.FillPath(priceBgBrush, pricePath);
                    
                    // Add border to price box
                    using (Pen priceBorderPen = new Pen(Color.FromArgb(190, 190, 200), 1))
                    {
                        g.DrawPath(priceBorderPen, pricePath);
                    }
                }
                
                // Draw price with currency symbol with improved styling
                using (Font priceFont = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush priceBrush = new SolidBrush(Color.FromArgb(20, 110, 20)))
                {
                    StringFormat priceFormat = new StringFormat();
                    priceFormat.Alignment = StringAlignment.Center;
                    priceFormat.LineAlignment = StringAlignment.Center;
                    
                    g.DrawString($"${product.Price:F2}", priceFont, priceBrush, priceBox, priceFormat);
                }
                
                // Draw stock info with improved visual representation
                int stockIndicatorSize = 12;
                Color stockColor = product.StockQuantity > 10 ? Color.FromArgb(0, 160, 0) : 
                                  product.StockQuantity > 0 ? Color.FromArgb(240, 150, 0) : 
                                  Color.FromArgb(200, 40, 40);
                
                // Draw stock indicator with gradient and glow effect
                Rectangle stockIndicatorRect = new Rectangle(15, 130, stockIndicatorSize, stockIndicatorSize);
                using (GraphicsPath stockPath = new GraphicsPath())
                {
                    stockPath.AddEllipse(stockIndicatorRect);
                    
                    // Fill with gradient for better look
                    using (PathGradientBrush stockBrush = new PathGradientBrush(stockPath))
                    {
                        Color centerColor = Color.FromArgb(255, stockColor);
                        Color[] surroundColors = { Color.FromArgb(200, stockColor) };
                        
                        stockBrush.CenterColor = centerColor;
                        stockBrush.SurroundColors = surroundColors;
                        
                        g.FillPath(stockBrush, stockPath);
                    }
                    
                    // Add highlight to indicator
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
                
                // Draw stock text with improved formatting
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
            // Do NOT use default drawing - we'll handle everything manually
            e.DrawDefault = false;
            
            // Get product from tag
            if (e.Item.Tag is Product product)
            {
                // Set consistent margins for each item
                const int margin = 5;
                
                // Calculate item rectangle (just for the image area, not including text)
                int imageHeight = lvProducts.LargeImageList.ImageSize.Height;
                var imageRect = new Rectangle(
                    e.Bounds.X + margin,
                    e.Bounds.Y + margin,
                    e.Bounds.Width - (margin * 2),
                    imageHeight
                );
                
                // Draw background with semi-transparency
                if ((e.State & ListViewItemStates.Selected) != 0)
                {
                    // Draw a selection effect with rounded corners
                    using (var path = CreateRoundedRectangle(imageRect, 8))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        
                        // Semi-transparent selection background
                        using (var brush = new LinearGradientBrush(
                            imageRect,
                            Color.FromArgb(100, 220, 230, 250), // More transparent
                            Color.FromArgb(100, 180, 200, 240), // More transparent
                            45F))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        
                        // Selection border
                        using (var pen = new Pen(Color.FromArgb(150, 100, 150, 200), 2))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
                else
                {
                    // Draw a subtle background with rounded corners
                    using (var path = CreateRoundedRectangle(imageRect, 8))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        
                        // Semi-transparent regular background
                        using (var brush = new LinearGradientBrush(
                            imageRect,
                            Color.FromArgb(80, 250, 250, 255), // Very transparent
                            Color.FromArgb(80, 240, 240, 250), // Very transparent
                            45F))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        
                        // Subtle border
                        using (var pen = new Pen(Color.FromArgb(120, 220, 220, 240), 1))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
                
                // Draw the product image
                if (e.Item.ImageKey != null && lvProducts.LargeImageList != null)
                {
                    Image img = lvProducts.LargeImageList.Images[e.Item.ImageKey];
                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, imageRect);
                    }
                }
                
                // Draw the text below the image
                string itemText = product.Name;
                Font textFont = e.Item.Font ?? lvProducts.Font;
                Brush textBrush = (e.State & ListViewItemStates.Selected) != 0 
                    ? new SolidBrush(Color.FromArgb(0, 70, 140))
                    : new SolidBrush(Color.FromArgb(60, 60, 80));
                
                // Calculate text position (centered below the image)
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
                // Allow selecting quantity beyond stock level
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

                    // Show processing cursor
                    this.Cursor = Cursors.WaitCursor;
                    
                    // Save sale to database - this will also update stock quantities
                    await _saleService.CreateSale(sale);
                    
                    // Refresh the product list to show updated stock
                    await LoadCategories();
                    LoadProducts();

                    // Clear cart and UI
                    _cartService.ClearCart();
                    UpdateCartDisplay();
                    UpdateTotal();
                    
                    // Restore cursor
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
                // Get categories from the service
                categories = await _productService.GetCategoriesAsync();
                
                // Clear existing category buttons except "All"
                foreach (var kvp in categoryButtons)
                {
                    if (kvp.Key != 0) // Keep the "All" button
                    {
                        pnlCategoryFilter.Controls.Remove(kvp.Value);
                        kvp.Value.Dispose();
                    }
                }
                
                // Clear the dictionary
                categoryButtons.Clear();
                
                // Add "All" button to dictionary
                categoryButtons[0] = btnAllCategories;
                
                // Create buttons for each category
                int xPosition = 70; // Start position after "All" button
                
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
                    
                    xPosition += 75; // Increment position for next button
                }
                
                // Update styles based on current category
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
            
            // Set current category
            _selectedCategoryId = categoryId;
            
            // Update button styles
            UpdateCategoryButtonStyles();
            
            // Load products for the selected category
            LoadProducts(categoryId: categoryId);
        }

        private void UpdateCategoryButtonStyles()
        {
            // Reset all buttons
            btnAllCategories.BackColor = Color.FromArgb(240, 240, 240);
            btnAllCategories.ForeColor = Color.Black;
            
            foreach (var button in categoryButtons.Values)
            {
                button.BackColor = Color.FromArgb(240, 240, 240);
                button.ForeColor = Color.Black;
            }

            // Set selected button
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
                // Left click - add to cart
                AddProductToCart(product);
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Right click - show context menu for managers
                var contextMenu = new ContextMenuStrip();
                
                // Only add Edit/Delete options for managers
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
            // Create a small colored icon for the menu items
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
                // Create product form for editing
                var productForm = new ProductForm(_productService, product);
                
                // Show the form
                if (productForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the product list
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
        
            // Show confirmation dialog with product details
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
                MessageBoxDefaultButton.Button2); // Default to "No"
        
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Use cursor to indicate operation in progress
                    Cursor = Cursors.WaitCursor;
                    
                    // Delete the product
                    bool success = await _productService.DeleteProduct(product.Id);
                    
                    // Reset cursor
                    Cursor = Cursors.Default;
                    
                    if (success)
                    {
                        // Refresh the product list
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
                // Open product form for managers
                var productForm = new ProductForm(_productService);
                if (productForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh categories and products
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
                // Open category form for managers
                var categoryForm = new CategoryForm(_productService);
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh categories and products
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
                this.Close(); // Close this form, which will trigger FormClosed event in LoginForm
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
            
            // Update button appearance based on cart state
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
                // Add product to cart using the cart service
                _cartService.AddToCart(product);
                
                // Check if the cart quantity exceeds stock
                var cartItem = _cartService.GetCart().Items.Find(i => i.ProductId == product.Id);
                if (cartItem != null && cartItem.Quantity > product.StockQuantity && product.StockQuantity > 0)
                {
                    // Inform the user, but allow proceeding with the order
                    MessageBox.Show(
                        $"You've ordered {cartItem.Quantity} units of {product.Name}, but only {product.StockQuantity} are in stock.\n\n" +
                        "Your order can still be processed, but may require additional time for the extra items.",
                        "Stock Notice",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                
                // Update the UI
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
            // Set current category to null (all categories)
            _selectedCategoryId = null;
            
            // Update button styles
            UpdateCategoryButtonStyles();
            
            // Load all products
            LoadProducts();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // Dispose of the DbContext
            _dbContext?.Dispose();
        }

        private void ConfigureUIForUserRole()
        {
            // Configure menu access based on user role
            if (_currentUser.Role == PosLibrary.Models.UserRole.Manager)
            {
                // Managers have access to all menu items
                menuCategory.Visible = true;
                menuProduct.Visible = true;
            }
            else
            {
                // Cashiers have limited access
                menuCategory.Visible = false;
                menuProduct.Visible = false;
            }
        }

        private void UpdateCartDisplay()
        {
            try
            {
                // Clear existing controls
                flowLayoutPanel1.Controls.Clear();
                
                // Get current cart
                var cart = _cartService.GetCart();
                
                // Add items to the panel
                foreach (var item in cart.Items)
                {
                    // Create panel for this item with rounded corners and shadow effect
                    var panel = new Panel
                    {
                        Size = new Size(580, 60),
                        Margin = new Padding(0, 0, 0, 10),
                        Tag = item
                    };
                    
                    // Custom painting for panel
                    panel.Paint += (s, e) => 
                    {
                        var p = (Panel)s;
                        var g = e.Graphics;
                        
                        // Draw rounded rectangle with gradient background
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
                            
                            // Draw border
                            using (var pen = new Pen(Color.FromArgb(200, 200, 230), 1))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                    };
                    
                    // Product name with custom font and styling
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
                    
                    // Quantity with custom styling
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
                    
                    // Unit price with custom styling
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
                    
                    // Subtotal with custom styling
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
                    
                    // Quantity adjustment buttons
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
                            // Get the product to check stock levels
                            var product = _productService.GetProductById(productId).GetAwaiter().GetResult();
                            
                            // Always increase quantity
                            _cartService.UpdateQuantity(productId, cartItem.Quantity + 1);
                            
                            // Show notification if exceeding stock
                            if (product != null && cartItem.Quantity + 1 > product.StockQuantity && product.StockQuantity > 0 
                                && cartItem.Quantity == product.StockQuantity + 1) // Only show once when exceeding stock
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
                    
                    // Remove button with custom styling
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
                    
                    // Add to flow layout
                    flowLayoutPanel1.Controls.Add(panel);
                }
                
                // Update total
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

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
} 