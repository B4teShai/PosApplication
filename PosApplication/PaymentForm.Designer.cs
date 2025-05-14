namespace PosApplication
{
    partial class PaymentForm
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
            this.totalLabel = new System.Windows.Forms.Label();
            this.amountPaidLabel = new System.Windows.Forms.Label();
            this.changeLabel = new System.Windows.Forms.Label();
            this.amountPaidNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.amountPaidNumericUpDown)).BeginInit();
            this.SuspendLayout();
            
            // Form properties
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            
            // totalLabel
            this.totalLabel.AutoSize = true;
            this.totalLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.totalLabel.Location = new System.Drawing.Point(12, 15);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(200, 25);
            this.totalLabel.TabIndex = 0;
            this.totalLabel.Text = "Total Amount: $0";
            this.totalLabel.ForeColor = System.Drawing.Color.FromArgb(50, 50, 100);
            
            // amountPaidLabel
            this.amountPaidLabel.AutoSize = true;
            this.amountPaidLabel.Location = new System.Drawing.Point(12, 55);
            this.amountPaidLabel.Name = "amountPaidLabel";
            this.amountPaidLabel.Size = new System.Drawing.Size(100, 15);
            this.amountPaidLabel.TabIndex = 1;
            this.amountPaidLabel.Text = "Amount Paid:";
            this.amountPaidLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.amountPaidLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            
            // changeLabel
            this.changeLabel.AutoSize = true;
            this.changeLabel.Location = new System.Drawing.Point(12, 85);
            this.changeLabel.Name = "changeLabel";
            this.changeLabel.Size = new System.Drawing.Size(100, 15);
            this.changeLabel.TabIndex = 2;
            this.changeLabel.Text = "Change: $0";
            this.changeLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.changeLabel.ForeColor = System.Drawing.Color.FromArgb(0, 100, 0);
            
            // amountPaidNumericUpDown
            this.amountPaidNumericUpDown.DecimalPlaces = 2;
            this.amountPaidNumericUpDown.Location = new System.Drawing.Point(118, 53);
            this.amountPaidNumericUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            this.amountPaidNumericUpDown.Name = "amountPaidNumericUpDown";
            this.amountPaidNumericUpDown.Size = new System.Drawing.Size(200, 23);
            this.amountPaidNumericUpDown.TabIndex = 3;
            this.amountPaidNumericUpDown.ThousandsSeparator = true;
            this.amountPaidNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.amountPaidNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.amountPaidNumericUpDown.ValueChanged += new System.EventHandler(this.amountPaidNumericUpDown_ValueChanged);
            
            // btnPay
            this.btnPay.Location = new System.Drawing.Point(118, 114);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(95, 35);
            this.btnPay.TabIndex = 4;
            this.btnPay.Text = "Pay";
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.BackColor = System.Drawing.Color.FromArgb(100, 150, 200);
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            
            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(223, 114);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(220, 220, 240);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // PaymentForm
            this.AcceptButton = this.btnPay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 165);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.amountPaidNumericUpDown);
            this.Controls.Add(this.changeLabel);
            this.Controls.Add(this.amountPaidLabel);
            this.Controls.Add(this.totalLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payment";
            ((System.ComponentModel.ISupportInitialize)(this.amountPaidNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.Label amountPaidLabel;
        private System.Windows.Forms.Label changeLabel;
        private System.Windows.Forms.NumericUpDown amountPaidNumericUpDown;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnCancel;
    }
} 