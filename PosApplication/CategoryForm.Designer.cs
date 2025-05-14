namespace PosApplication
{
    partial class CategoryForm
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstCategories = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.lblCategoryList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // Form properties
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // lblCategoryList
            this.lblCategoryList.AutoSize = true;
            this.lblCategoryList.Location = new System.Drawing.Point(20, 20);
            this.lblCategoryList.Name = "lblCategoryList";
            this.lblCategoryList.Size = new System.Drawing.Size(120, 20);
            this.lblCategoryList.TabIndex = 0;
            this.lblCategoryList.Text = "Categories:";
            this.lblCategoryList.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCategoryList.ForeColor = System.Drawing.Color.FromArgb(50, 50, 100);
            
            // lstCategories
            this.lstCategories.FormattingEnabled = true;
            this.lstCategories.ItemHeight = 16;
            this.lstCategories.Location = new System.Drawing.Point(20, 45);
            this.lstCategories.Name = "lstCategories";
            this.lstCategories.Size = new System.Drawing.Size(200, 260);
            this.lstCategories.TabIndex = 1;
            this.lstCategories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCategories.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstCategories.BackColor = System.Drawing.Color.White;
            this.lstCategories.SelectedIndexChanged += new System.EventHandler(this.lstCategories_SelectedIndexChanged);
            
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(240, 45);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Category Name:";
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // txtName
            this.txtName.Location = new System.Drawing.Point(350, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 25);
            this.txtName.TabIndex = 3;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(240, 80);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 20);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(350, 80);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(250, 100);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.Multiline = true;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // chkActive
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(350, 190);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(100, 20);
            this.chkActive.TabIndex = 6;
            this.chkActive.Text = "Active";
            this.chkActive.Checked = true;
            this.chkActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkActive.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(350, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 35);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnNew
            this.btnNew.Location = new System.Drawing.Point(20, 320);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 35);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "New";
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(120, 170, 120);
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(130, 320);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 35);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(220, 100, 100);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnDelete.Enabled = false;
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(480, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Close";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 220, 240);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            
            // CategoryForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 370);
            this.Controls.Add(this.lblCategoryList);
            this.Controls.Add(this.lstCategories);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Category Management";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCategoryList;
        private System.Windows.Forms.ListBox lstCategories;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
    }
} 