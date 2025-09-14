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
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            chkActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lstCategories = new ListBox();
            btnDelete = new Button();
            btnNew = new Button();
            lblCategoryList = new Label();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(80, 80, 120);
            lblName.Location = new Point(274, 60);
            lblName.Name = "lblName";
            lblName.Size = new Size(123, 20);
            lblName.TabIndex = 2;
            lblName.Text = "Category Name:";
            // 
            // txtName
            // 
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 9F);
            txtName.Location = new Point(400, 60);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.Size = new Size(285, 27);
            txtName.TabIndex = 3;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDescription.ForeColor = Color.FromArgb(80, 80, 120);
            lblDescription.Location = new Point(274, 107);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(93, 20);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Font = new Font("Segoe UI", 9F);
            txtDescription.Location = new Point(400, 107);
            txtDescription.Margin = new Padding(3, 4, 3, 4);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(285, 133);
            txtDescription.TabIndex = 5;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Font = new Font("Segoe UI", 9F);
            chkActive.ForeColor = Color.FromArgb(80, 80, 120);
            chkActive.Location = new Point(400, 253);
            chkActive.Margin = new Padding(3, 4, 3, 4);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(72, 24);
            chkActive.TabIndex = 6;
            chkActive.Text = "Active";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(100, 150, 200);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(400, 307);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(137, 47);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 220, 240);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.FromArgb(80, 80, 120);
            btnCancel.Location = new Point(549, 307);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(137, 47);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Close";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnClose_Click;
            // 
            // lstCategories
            // 
            lstCategories.BackColor = Color.White;
            lstCategories.BorderStyle = BorderStyle.FixedSingle;
            lstCategories.Font = new Font("Segoe UI", 9F);
            lstCategories.FormattingEnabled = true;
            lstCategories.Location = new Point(23, 60);
            lstCategories.Margin = new Padding(3, 4, 3, 4);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(228, 342);
            lstCategories.TabIndex = 1;
            lstCategories.SelectedIndexChanged += lstCategories_SelectedIndexChanged;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 100, 100);
            btnDelete.Enabled = false;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(149, 427);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(103, 47);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.FromArgb(120, 170, 120);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNew.ForeColor = Color.White;
            btnNew.Location = new Point(23, 427);
            btnNew.Margin = new Padding(3, 4, 3, 4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(103, 47);
            btnNew.TabIndex = 8;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // lblCategoryList
            // 
            lblCategoryList.AutoSize = true;
            lblCategoryList.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategoryList.ForeColor = Color.FromArgb(50, 50, 100);
            lblCategoryList.Location = new Point(23, 27);
            lblCategoryList.Name = "lblCategoryList";
            lblCategoryList.Size = new Size(100, 23);
            lblCategoryList.TabIndex = 0;
            lblCategoryList.Text = "Categories:";
            // 
            // CategoryForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 250, 255);
            ClientSize = new Size(709, 493);
            Controls.Add(lblCategoryList);
            Controls.Add(lstCategories);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(chkActive);
            Controls.Add(btnSave);
            Controls.Add(btnNew);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CategoryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Category Management";
            ResumeLayout(false);
            PerformLayout();
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