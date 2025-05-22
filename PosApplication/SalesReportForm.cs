using System;
using System.Windows.Forms;
using System.Linq;
using PosLibrary.Models;
using PosLibrary.Services;
using System.Collections.Generic;
using System.Drawing;

namespace PosApplication
{
    public partial class SalesReportForm : Form
    {
        private readonly SaleService _saleService;
        private readonly ReceiptService _receiptService;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private DataGridView dgvSales;
        private Label lblTotalSales;
        private Label lblTotalItems;
        private Button btnGenerate;
        private Button btnExport;

        public SalesReportForm(SaleService saleService, ReceiptService receiptService)
        {
            InitializeComponent();
            _saleService = saleService;
            _receiptService = receiptService;
            InitializeCustomComponents();
            LoadSalesReport();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Sales Report";
            this.Size = new Size(1000, 600);

            // Date range controls
            var lblStartDate = new Label { Text = "Start Date:", Location = new Point(20, 20) };
            dtpStartDate = new DateTimePicker 
            { 
                Location = new Point(100, 20),
                Format = DateTimePickerFormat.Short
            };

            var lblEndDate = new Label { Text = "End Date:", Location = new Point(300, 20) };
            dtpEndDate = new DateTimePicker 
            { 
                Location = new Point(380, 20),
                Format = DateTimePickerFormat.Short
            };

            // DataGridView for sales
            dgvSales = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(940, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Summary labels
            lblTotalSales = new Label 
            { 
                Text = "Total Sales: $0.00",
                Location = new Point(20, 480),
                Font = new Font(this.Font, FontStyle.Bold)
            };

            lblTotalItems = new Label 
            { 
                Text = "Total Items Sold: 0",
                Location = new Point(20, 510),
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // Buttons
            btnGenerate = new Button
            {
                Text = "Generate Report",
                Location = new Point(700, 20),
                Size = new Size(120, 30)
            };
            btnGenerate.Click += BtnGenerate_Click;

            btnExport = new Button
            {
                Text = "Export to CSV",
                Location = new Point(840, 20),
                Size = new Size(120, 30)
            };
            btnExport.Click += BtnExport_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] 
            { 
                lblStartDate, dtpStartDate, 
                lblEndDate, dtpEndDate,
                dgvSales, lblTotalSales, lblTotalItems,
                btnGenerate, btnExport
            });
        }

        private void LoadSalesReport()
        {
            var startDate = dtpStartDate.Value.Date;
            var endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            var sales = _saleService.GetSalesByDateRange(startDate, endDate);
            
            dgvSales.DataSource = sales.Select(s => new
            {
                s.Id,
                Date = s.CreatedAt,
                Items = s.Items.Count,
                Total = s.Total,
                Cashier = s.User?.Username ?? "Unknown"
            }).ToList();

            // Calculate totals
            decimal totalSales = sales.Sum(s => s.Total);
            int totalItems = sales.Sum(s => s.Items.Count);

            lblTotalSales.Text = $"Total Sales: ${totalSales:F2}";
            lblTotalItems.Text = $"Total Items Sold: {totalItems}";
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv";
                saveDialog.Title = "Export Sales Report";
                saveDialog.FileName = $"SalesReport_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sales = (IEnumerable<dynamic>)dgvSales.DataSource;
                        var lines = new List<string>();
                        
                        // Add headers
                        lines.Add("ID,Date,Items,Total,Cashier");
                        
                        // Add data
                        foreach (var sale in sales)
                        {
                            lines.Add($"{sale.Id},{sale.Date},{sale.Items},${sale.Total:F2},{sale.Cashier}");
                        }

                        System.IO.File.WriteAllLines(saveDialog.FileName, lines);
                        MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
} 