namespace PosApplication
{
    partial class ManagerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabProducts = new System.Windows.Forms.TabPage();
            this.tabCategories = new System.Windows.Forms.TabPage();
            this.tabReports = new System.Windows.Forms.TabPage();
            
            // Product Tab Controls
            this.productDataGridView = new System.Windows.Forms.DataGridView();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnEditProduct = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            
            // Category Tab Controls
            this.categoryDataGridView = new System.Windows.Forms.DataGridView();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.btnEditCategory = new System.Windows.Forms.Button();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            
            // Reports Tab Controls
            this.helpTextBox = new System.Windows.Forms.TextBox();
            
            // TabControl
            this.tabControl.SuspendLayout();
            
            // Products Tab
            this.tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productDataGridView)).BeginInit();
            
            // Categories Tab
            this.tabCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDataGridView)).BeginInit();
            
            // Reports Tab
            this.tabReports.SuspendLayout();
            
            this.SuspendLayout();
            
            // Form properties
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // tabControl
            this.tabControl.Controls.Add(this.tabProducts);
            this.tabControl.Controls.Add(this.tabCategories);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.TabIndex = 0;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // tabProducts
            this.tabProducts.Controls.Add(this.productDataGridView);
            this.tabProducts.Controls.Add(this.btnAddProduct);
            this.tabProducts.Controls.Add(this.btnEditProduct);
            this.tabProducts.Controls.Add(this.btnDeleteProduct);
            this.tabProducts.Location = new System.Drawing.Point(4, 24);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabProducts.Size = new System.Drawing.Size(792, 422);
            this.tabProducts.TabIndex = 0;
            this.tabProducts.Text = "Products";
            this.tabProducts.BackColor = System.Drawing.Color.FromArgb(245, 245, 255);
            
            // productDataGridView
            this.productDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productDataGridView.Location = new System.Drawing.Point(8, 6);
            this.productDataGridView.Name = "productDataGridView";
            this.productDataGridView.Size = new System.Drawing.Size(776, 350);
            this.productDataGridView.TabIndex = 0;
            this.productDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.productDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productDataGridView.GridColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.productDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.productDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            
            // btnAddProduct
            this.btnAddProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddProduct.Location = new System.Drawing.Point(8, 380);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(90, 35);
            this.btnAddProduct.TabIndex = 1;
            this.btnAddProduct.Text = "Add";
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.BackColor = System.Drawing.Color.FromArgb(120, 170, 120);
            this.btnAddProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            
            // btnEditProduct
            this.btnEditProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditProduct.Location = new System.Drawing.Point(108, 380);
            this.btnEditProduct.Name = "btnEditProduct";
            this.btnEditProduct.Size = new System.Drawing.Size(90, 35);
            this.btnEditProduct.TabIndex = 2;
            this.btnEditProduct.Text = "Edit";
            this.btnEditProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditProduct.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnEditProduct.ForeColor = System.Drawing.Color.White;
            this.btnEditProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditProduct.Click += new System.EventHandler(this.btnEditProduct_Click);
            
            // btnDeleteProduct
            this.btnDeleteProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteProduct.Location = new System.Drawing.Point(208, 380);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(90, 35);
            this.btnDeleteProduct.TabIndex = 3;
            this.btnDeleteProduct.Text = "Delete";
            this.btnDeleteProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProduct.BackColor = System.Drawing.Color.FromArgb(220, 100, 100);
            this.btnDeleteProduct.ForeColor = System.Drawing.Color.White;
            this.btnDeleteProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            
            // tabCategories
            this.tabCategories.Controls.Add(this.categoryDataGridView);
            this.tabCategories.Controls.Add(this.btnAddCategory);
            this.tabCategories.Controls.Add(this.btnEditCategory);
            this.tabCategories.Controls.Add(this.btnDeleteCategory);
            this.tabCategories.Location = new System.Drawing.Point(4, 24);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategories.Size = new System.Drawing.Size(792, 422);
            this.tabCategories.TabIndex = 1;
            this.tabCategories.Text = "Categories";
            this.tabCategories.BackColor = System.Drawing.Color.FromArgb(245, 245, 255);
            
            // categoryDataGridView
            this.categoryDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.categoryDataGridView.Location = new System.Drawing.Point(8, 6);
            this.categoryDataGridView.Name = "categoryDataGridView";
            this.categoryDataGridView.Size = new System.Drawing.Size(776, 350);
            this.categoryDataGridView.TabIndex = 0;
            this.categoryDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.categoryDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.categoryDataGridView.GridColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.categoryDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.categoryDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            
            // btnAddCategory
            this.btnAddCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCategory.Location = new System.Drawing.Point(8, 380);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(90, 35);
            this.btnAddCategory.TabIndex = 1;
            this.btnAddCategory.Text = "Add";
            this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCategory.BackColor = System.Drawing.Color.FromArgb(120, 170, 120);
            this.btnAddCategory.ForeColor = System.Drawing.Color.White;
            this.btnAddCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            
            // btnEditCategory
            this.btnEditCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditCategory.Location = new System.Drawing.Point(108, 380);
            this.btnEditCategory.Name = "btnEditCategory";
            this.btnEditCategory.Size = new System.Drawing.Size(90, 35);
            this.btnEditCategory.TabIndex = 2;
            this.btnEditCategory.Text = "Edit";
            this.btnEditCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditCategory.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnEditCategory.ForeColor = System.Drawing.Color.White;
            this.btnEditCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditCategory.Click += new System.EventHandler(this.btnEditCategory_Click);
            
            // btnDeleteCategory
            this.btnDeleteCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteCategory.Location = new System.Drawing.Point(208, 380);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(90, 35);
            this.btnDeleteCategory.TabIndex = 3;
            this.btnDeleteCategory.Text = "Delete";
            this.btnDeleteCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCategory.BackColor = System.Drawing.Color.FromArgb(220, 100, 100);
            this.btnDeleteCategory.ForeColor = System.Drawing.Color.White;
            this.btnDeleteCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            
            // tabReports
            this.tabReports.Controls.Add(this.helpTextBox);
            this.tabReports.Location = new System.Drawing.Point(4, 24);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(792, 422);
            this.tabReports.TabIndex = 2;
            this.tabReports.Text = "Reports";
            this.tabReports.BackColor = System.Drawing.Color.FromArgb(245, 245, 255);
            
            // helpTextBox
            this.helpTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpTextBox.Location = new System.Drawing.Point(3, 3);
            this.helpTextBox.Multiline = true;
            this.helpTextBox.Name = "helpTextBox";
            this.helpTextBox.ReadOnly = true;
            this.helpTextBox.Size = new System.Drawing.Size(786, 416);
            this.helpTextBox.TabIndex = 0;
            this.helpTextBox.Text = "Help text will be displayed here.";
            this.helpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.helpTextBox.BackColor = System.Drawing.Color.White;
            
            // ManagerForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Name = "ManagerForm";
            this.Text = "Manager Dashboard";
            this.tabControl.ResumeLayout(false);
            this.tabProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productDataGridView)).EndInit();
            this.tabCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.categoryDataGridView)).EndInit();
            this.tabReports.ResumeLayout(false);
            this.tabReports.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.TabPage tabCategories;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.DataGridView productDataGridView;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnEditProduct;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.DataGridView categoryDataGridView;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Button btnEditCategory;
        private System.Windows.Forms.Button btnDeleteCategory;
        private System.Windows.Forms.TextBox helpTextBox;
    }
} 