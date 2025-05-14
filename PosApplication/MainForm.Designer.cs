namespace PosApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "POS System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            // Menu Strip
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.menuStrip.ForeColor = System.Drawing.Color.White;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // Product Menu
            this.menuProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProduct.Text = "Product";
            this.menuProduct.Click += new System.EventHandler(this.MenuProduct_Click);

            // Category Menu
            this.menuCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCategory.Text = "Category";
            this.menuCategory.Click += new System.EventHandler(this.MenuCategory_Click);

            // Help Menu
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp.Text = "Help";
            this.menuHelp.Click += new System.EventHandler(this.MenuHelp_Click);
            
            // Logout Menu
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout.Text = "Logout";
            this.menuLogout.Click += new System.EventHandler(this.MenuLogout_Click);

            // Add menus to MenuStrip
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuProduct,
                this.menuCategory,
                this.menuHelp,
                this.menuLogout
            });

            // Search TextBox
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtSearch.Location = new System.Drawing.Point(12, 30);
            this.txtSearch.Size = new System.Drawing.Size(600, 25);
            this.txtSearch.PlaceholderText = "Search products...";
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Goods Code Entry Field
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.txtGoodsCode.Location = new System.Drawing.Point(12, 70);
            this.txtGoodsCode.Size = new System.Drawing.Size(200, 25);
            this.txtGoodsCode.PlaceholderText = "Enter goods code...";
            this.txtGoodsCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtGoodsCode_KeyDown);
            this.txtGoodsCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Goods Code Label
            this.lblGoodsCode = new System.Windows.Forms.Label();
            this.lblGoodsCode.Text = "Goods Code:";
            this.lblGoodsCode.Location = new System.Drawing.Point(220, 73);
            this.lblGoodsCode.AutoSize = true;
            this.lblGoodsCode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGoodsCode.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);

            // Add Button
            this.btnAddByCode = new System.Windows.Forms.Button();
            this.btnAddByCode.Text = "Add";
            this.btnAddByCode.Location = new System.Drawing.Point(320, 70);
            this.btnAddByCode.Size = new System.Drawing.Size(80, 25);
            this.btnAddByCode.Click += new System.EventHandler(this.BtnAddByCode_Click);
            this.btnAddByCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddByCode.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnAddByCode.ForeColor = System.Drawing.Color.White;
            this.btnAddByCode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // Category Filter Panel
            this.pnlCategoryFilter = new System.Windows.Forms.Panel();
            this.pnlCategoryFilter.Location = new System.Drawing.Point(630, 30);
            this.pnlCategoryFilter.Size = new System.Drawing.Size(550, 30);
            this.pnlCategoryFilter.BackColor = System.Drawing.Color.FromArgb(240, 240, 250);
            this.pnlCategoryFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // All Categories Button
            this.btnAllCategories = new System.Windows.Forms.Button();
            this.btnAllCategories.Text = "All";
            this.btnAllCategories.Location = new System.Drawing.Point(5, 3);
            this.btnAllCategories.Size = new System.Drawing.Size(60, 24);
            this.btnAllCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllCategories.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnAllCategories.ForeColor = System.Drawing.Color.White;
            this.btnAllCategories.Click += new System.EventHandler(this.BtnAllCategories_Click);

            // Add buttons to category filter panel
            this.pnlCategoryFilter.Controls.Add(this.btnAllCategories);

            // Products ListView
            this.lvProducts = new System.Windows.Forms.ListView();
            this.lvProducts.Location = new System.Drawing.Point(630, 70);
            this.lvProducts.Size = new System.Drawing.Size(550, 680);
            this.lvProducts.View = System.Windows.Forms.View.LargeIcon;
            this.lvProducts.TileSize = new System.Drawing.Size(170, 170);
            this.lvProducts.FullRowSelect = true;
            this.lvProducts.LargeImageList = new System.Windows.Forms.ImageList();
            this.lvProducts.LargeImageList.ImageSize = new System.Drawing.Size(160, 160);
            this.lvProducts.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.lvProducts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lvProducts.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);
            this.lvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvProducts.MultiSelect = false;
            this.lvProducts.HideSelection = false;
            this.lvProducts.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvProducts.FullRowSelect = true;
            this.lvProducts.GridLines = false;
            this.lvProducts.ShowGroups = false;
            this.lvProducts.ShowItemToolTips = true;
            this.lvProducts.UseCompatibleStateImageBehavior = false;
            this.lvProducts.StateImageList = null;
            this.lvProducts.LabelWrap = true;
            this.lvProducts.LabelEdit = false;
            this.lvProducts.CheckBoxes = false;
            this.lvProducts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvProducts.OwnerDraw = true;
            this.lvProducts.Padding = new System.Windows.Forms.Padding(10);
            this.lvProducts.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.LvProducts_DrawItem);
            this.lvProducts.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.LvProducts_DrawSubItem);
            this.lvProducts.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.LvProducts_DrawColumnHeader);
            this.lvProducts.Alignment = System.Windows.Forms.ListViewAlignment.Top;
            this.lvProducts.AutoArrange = true;
            this.lvProducts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvProducts_MouseClick);

            // Total Label
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotal.Location = new System.Drawing.Point(12, 680);
            this.lblTotal.Text = "Total: $0";
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(50, 50, 100);

            // Complete Sale Button
            this.btnCompleteSale = new System.Windows.Forms.Button();
            this.btnCompleteSale.Text = "Complete Sale";
            this.btnCompleteSale.Location = new System.Drawing.Point(280, 720);
            this.btnCompleteSale.Size = new System.Drawing.Size(200, 40);
            this.btnCompleteSale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompleteSale.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnCompleteSale.ForeColor = System.Drawing.Color.White;
            this.btnCompleteSale.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCompleteSale.Click += new System.EventHandler(this.BtnCompleteSale_Click);

            // Flow Layout Panel for cart items
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 110);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(600, 560);
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);

            // Add controls to form
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.menuStrip,
                this.lvProducts,
                this.lblTotal,
                this.btnCompleteSale,
                this.txtSearch,
                this.pnlCategoryFilter,
                this.txtGoodsCode,
                this.lblGoodsCode,
                this.btnAddByCode,
                this.flowLayoutPanel1
            });
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuProduct;
        private System.Windows.Forms.ToolStripMenuItem menuCategory;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ListView lvProducts;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnCompleteSale;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlCategoryFilter;
        private System.Windows.Forms.Button btnAllCategories;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label lblGoodsCode;
        private System.Windows.Forms.Button btnAddByCode;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
} 