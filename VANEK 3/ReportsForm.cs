using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT s.Id, s.SaleDate, s.CustomerName, s.TotalAmount,
                                (SELECT COUNT(*) FROM SaleItems WHERE SaleId = s.Id) as ItemsCount
                                FROM Sales s
                                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                                ORDER BY s.SaleDate DESC";
                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                dataGridView.Columns["Id"].Visible = false;
                dataGridView.Columns["SaleDate"].HeaderText = "Дата продажи";
                dataGridView.Columns["SaleDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                dataGridView.Columns["CustomerName"].HeaderText = "Клиент";
                dataGridView.Columns["TotalAmount"].HeaderText = "Сумма";
                dataGridView.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["ItemsCount"].HeaderText = "Количество позиций";

                decimal total = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["TotalAmount"].Value != null)
                        total += Convert.ToDecimal(row.Cells["TotalAmount"].Value);
                }
                lblSummary.Text = $"Всего продаж: {dataGridView.Rows.Count} | Общая сумма: {total:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnServicesReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT fs.Name, fs.Category, fs.Price,
                                COUNT(si.Id) as SalesCount,
                                SUM(si.Quantity) as TotalQuantity,
                                SUM(si.TotalPrice) as TotalRevenue
                                FROM FinancialServices fs
                                LEFT JOIN SaleItems si ON fs.Id = si.ServiceId
                                LEFT JOIN Sales s ON si.SaleId = s.Id AND s.SaleDate BETWEEN @FromDate AND @ToDate
                                GROUP BY fs.Id, fs.Name, fs.Category, fs.Price
                                ORDER BY TotalRevenue DESC";
                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                dataGridView.Columns["Name"].HeaderText = "Наименование";
                dataGridView.Columns["Category"].HeaderText = "Категория";
                dataGridView.Columns["Price"].HeaderText = "Цена";
                dataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["SalesCount"].HeaderText = "Количество продаж";
                dataGridView.Columns["TotalQuantity"].HeaderText = "Общее количество";
                dataGridView.Columns["TotalRevenue"].HeaderText = "Общая выручка";
                dataGridView.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";

                decimal totalRevenue = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["TotalRevenue"].Value != null && row.Cells["TotalRevenue"].Value != DBNull.Value)
                        totalRevenue += Convert.ToDecimal(row.Cells["TotalRevenue"].Value);
                }
                lblSummary.Text = $"Всего услуг: {dataGridView.Rows.Count} | Общая выручка: {totalRevenue:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDailyReport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT CAST(s.SaleDate AS DATE) as SaleDate,
                                COUNT(s.Id) as SalesCount,
                                SUM(s.TotalAmount) as DailyTotal
                                FROM Sales s
                                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                                GROUP BY CAST(s.SaleDate AS DATE)
                                ORDER BY SaleDate DESC";
                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                dataGridView.Columns["SaleDate"].HeaderText = "Дата";
                dataGridView.Columns["SaleDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dataGridView.Columns["SalesCount"].HeaderText = "Количество продаж";
                dataGridView.Columns["DailyTotal"].HeaderText = "Сумма за день";
                dataGridView.Columns["DailyTotal"].DefaultCellStyle.Format = "C2";

                decimal total = 0;
                int count = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["DailyTotal"].Value != null)
                    {
                        total += Convert.ToDecimal(row.Cells["DailyTotal"].Value);
                        count += Convert.ToInt32(row.Cells["SalesCount"].Value);
                    }
                }
                lblSummary.Text = $"Всего дней: {dataGridView.Rows.Count} | Продаж: {count} | Общая сумма: {total:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

