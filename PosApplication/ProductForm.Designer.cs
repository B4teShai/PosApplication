namespace PosApplication
{
    partial class ProductForm
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
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.lblStock = new System.Windows.Forms.Label();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnBrowseImage = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            
            // Form properties
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // lblCode
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(20, 20);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(100, 20);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "Product Code:";
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // txtCode
            this.txtCode.Location = new System.Drawing.Point(130, 20);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(250, 25);
            this.txtCode.TabIndex = 1;
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 55);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Product Name:";
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // txtName
            this.txtName.Location = new System.Drawing.Point(130, 55);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 25);
            this.txtName.TabIndex = 3;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 90);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 20);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(130, 90);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(250, 60);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.Multiline = true;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // lblPrice
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 160);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(100, 20);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Price:";
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // numPrice
            this.numPrice.Location = new System.Drawing.Point(130, 160);
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(120, 25);
            this.numPrice.TabIndex = 7;
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Maximum = 10000000;
            this.numPrice.ThousandsSeparator = true;
            this.numPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // lblStock
            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(20, 195);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(100, 20);
            this.lblStock.TabIndex = 8;
            this.lblStock.Text = "Stock Quantity:";
            this.lblStock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStock.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // numStock
            this.numStock.Location = new System.Drawing.Point(130, 195);
            this.numStock.Name = "numStock";
            this.numStock.Size = new System.Drawing.Size(120, 25);
            this.numStock.TabIndex = 9;
            this.numStock.Maximum = 1000000;
            this.numStock.ThousandsSeparator = true;
            this.numStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // lblCategory
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 230);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(100, 20);
            this.lblCategory.TabIndex = 10;
            this.lblCategory.Text = "Category:";
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // cmbCategory
            this.cmbCategory.Location = new System.Drawing.Point(130, 230);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(250, 25);
            this.cmbCategory.TabIndex = 11;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            
            // chkActive
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(130, 265);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(100, 20);
            this.chkActive.TabIndex = 12;
            this.chkActive.Text = "Active";
            this.chkActive.Checked = true;
            this.chkActive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkActive.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // lblImage
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(20, 300);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(100, 20);
            this.lblImage.TabIndex = 13;
            this.lblImage.Text = "Product Image:";
            this.lblImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblImage.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // pictureBox
            this.pictureBox.Location = new System.Drawing.Point(130, 300);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.TabIndex = 14;
            
            // btnBrowseImage
            this.btnBrowseImage.Location = new System.Drawing.Point(290, 300);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(90, 30);
            this.btnBrowseImage.TabIndex = 15;
            this.btnBrowseImage.Text = "Browse...";
            this.btnBrowseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseImage.BackColor = System.Drawing.Color.FromArgb(220, 220, 240);
            this.btnBrowseImage.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnBrowseImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(130, 470);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 35);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(260, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 220, 240);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // ProductForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 520);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.numStock);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnBrowseImage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product";
            
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.NumericUpDown numStock;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 