namespace PosApplication
{
    partial class LoginForm
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
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Text = "POS Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Form properties
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            // Title Label
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "POS System Login";
            this.lblTitle.Location = new System.Drawing.Point(100, 30);
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 100);

            // Username Label
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUsername.Text = "Username:";
            this.lblUsername.Location = new System.Drawing.Point(50, 80);
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);

            // Username TextBox
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtUsername.Location = new System.Drawing.Point(150, 80);
            this.txtUsername.Size = new System.Drawing.Size(200, 25);
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9F);

            // Password Label
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(50, 120);
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);

            // Password TextBox
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPassword.Location = new System.Drawing.Point(150, 120);
            this.txtPassword.Size = new System.Drawing.Size(200, 25);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F);

            // Login Button
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(150, 170);
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            // Exit Button
            this.btnExit = new System.Windows.Forms.Button();
            this.btnExit.Text = "Exit";
            this.btnExit.Location = new System.Drawing.Point(260, 170);
            this.btnExit.Size = new System.Drawing.Size(90, 35);
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(220, 220, 240);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            // Add controls to form
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitle,
                this.lblUsername,
                this.txtUsername,
                this.lblPassword,
                this.txtPassword,
                this.btnLogin,
                this.btnExit
            });
        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
    }
} 