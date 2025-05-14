using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using PosLibrary.Models;
using PosLibrary.Services;

namespace PosApplication
{
    public partial class CategoryForm : Form
    {
        private readonly ProductService _productService;
        private Category _selectedCategory;

        public CategoryForm(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _productService.GetAllCategories();
                lstCategories.DataSource = null;
                lstCategories.DataSource = categories;
                lstCategories.DisplayMember = "Name";
                lstCategories.ValueMember = "Id";
                
                // Clear form fields
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            _selectedCategory = null;
            btnSave.Text = "Add";
            btnDelete.Enabled = false;
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem is Category category)
            {
                _selectedCategory = category;
                txtName.Text = category.Name;
                txtDescription.Text = category.Description;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    bool success;
                    
                    if (_selectedCategory == null)
                    {
                        // Add new category
                        var newCategory = new Category
                        {
                            Name = txtName.Text.Trim(),
                            Description = txtDescription.Text.Trim()
                        };
                        
                        success = await _productService.AddCategory(newCategory);
                        if (success)
                        {
                            MessageBox.Show("Category added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        // Update existing category
                        _selectedCategory.Name = txtName.Text.Trim();
                        _selectedCategory.Description = txtDescription.Text.Trim();
                        
                        success = await _productService.UpdateCategory(_selectedCategory);
                        if (success)
                        {
                            MessageBox.Show("Category updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    
                    if (success)
                    {
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save category. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCategory != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete the category '{_selectedCategory.Name}'?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        bool success = await _productService.DeleteCategory(_selectedCategory.Id);
                        if (success)
                        {
                            MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete category. It may be in use by products.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a category description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            return true;
        }
    }
} 