using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using PosLibrary.Models;
using PosLibrary.Services;

namespace PosApplication
{
    public partial class ProductForm : Form
    {
        private readonly ProductService _productService;
        private Product _product;
        private bool _isEditMode;
        private string _selectedImagePath;

        public ProductForm(ProductService productService, Product product = null)
        {
            InitializeComponent();
            _productService = productService;
            
            if (product != null)
            {
                _product = product;
                _isEditMode = true;
            }
            else
            {
                _product = new Product
                {
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _isEditMode = false;
            }

            LoadCategories();
            if (_isEditMode)
            {
                LoadProductData();
                this.Text = "Edit Product";
            }
            else
            {
                this.Text = "Add New Product";
            }
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _productService.GetAllCategories();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductData()
        {
            txtCode.Text = _product.Code;
            txtName.Text = _product.Name;
            txtDescription.Text = _product.Description;
            numPrice.Value = _product.Price;
            numStock.Value = _product.StockQuantity;
            chkActive.Checked = _product.IsActive;
            
            if (_product.CategoryId > 0)
            {
                cmbCategory.SelectedValue = _product.CategoryId;
            }

            // Load existing product image
            if (!string.IsNullOrEmpty(_product.ImagePath))
            {
                _selectedImagePath = _product.ImagePath;
                
                // Try to load the image from various possible paths
                string[] possiblePaths = {
                    _product.ImagePath,
                    Path.Combine(Application.StartupPath, "ProductImages", Path.GetFileName(_product.ImagePath)),
                    Path.Combine(Application.StartupPath, Path.GetFileName(_product.ImagePath)),
                    Path.Combine(Directory.GetCurrentDirectory(), "ProductImages", Path.GetFileName(_product.ImagePath))
                };
                
                bool imageLoaded = false;
                foreach (string testPath in possiblePaths)
                {
                    if (File.Exists(testPath))
                    {
                        try
                        {
                            pictureBox.Image = Image.FromFile(testPath);
                            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            _selectedImagePath = testPath; // Update to working path
                            imageLoaded = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading image {testPath}: {ex.Message}");
                        }
                    }
                }
                
                if (!imageLoaded)
                {
                    pictureBox.Image = null;
                    _selectedImagePath = null;
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Update product with form values
                    _product.Code = txtCode.Text;
                    _product.Name = txtName.Text;
                    _product.Description = txtDescription.Text;
                    _product.Price = numPrice.Value;
                    _product.StockQuantity = (int)numStock.Value;
                    _product.IsActive = chkActive.Checked;
                    _product.CategoryId = (int)cmbCategory.SelectedValue;
                    
                    if (!string.IsNullOrEmpty(_selectedImagePath))
                    {
                        // Ensure ProductImages directory exists in application directory
                        string targetDir = Path.Combine(Application.StartupPath, "ProductImages");
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        string fileExtension = Path.GetExtension(_selectedImagePath);
                        string fileName = $"product_{_product.Code}_{DateTime.Now:yyyyMMdd_HHmmss}{fileExtension}";
                        string targetPath = Path.Combine(targetDir, fileName);
                        
                        // Copy image to ProductImages folder if it's not already there
                        if (!_selectedImagePath.StartsWith(targetDir))
                        {
                            File.Copy(_selectedImagePath, targetPath, true);
                            _product.ImagePath = targetPath;
                        }
                        else
                        {
                            _product.ImagePath = _selectedImagePath;
                        }
                    }

                    bool success;
                    if (_isEditMode)
                    {
                        _product.UpdatedAt = DateTime.Now;
                        success = await _productService.UpdateProduct(_product);
                    }
                    else
                    {
                        success = await _productService.AddProduct(_product);
                    }

                    if (success)
                    {
                        MessageBox.Show(_isEditMode ? "Product updated successfully!" : "Product added successfully!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save product. Please try again.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving product: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Title = "Select Product Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = openFileDialog.FileName;
                    try
                    {
                        pictureBox.Image = Image.FromFile(_selectedImagePath);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Please enter a product code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (numPrice.Value <= 0)
            {
                MessageBox.Show("Price must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrice.Focus();
                return false;
            }

            return true;
        }
    }
}