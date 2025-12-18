using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SaleDetailsForm : Form
    {
        public SaleDetailsForm(int saleId)
        {
            InitializeComponent();
            LoadSaleDetails(saleId);
        }

        private void LoadSaleDetails(int saleId)
        {
            try
            {
                string query = @"SELECT s.SaleDate, s.CustomerName, s.TotalAmount, s.Notes
                                FROM Sales s WHERE s.Id = @SaleId";
                var saleData = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@SaleId", saleId));
                
                if (saleData.Rows.Count > 0)
                {
                    lblSaleDate.Text = $"Дата: {Convert.ToDateTime(saleData.Rows[0]["SaleDate"]):dd.MM.yyyy HH:mm}";
                    lblCustomerName.Text = $"Клиент: {saleData.Rows[0]["CustomerName"]}";
                    lblTotalAmount.Text = $"Итого: {Convert.ToDecimal(saleData.Rows[0]["TotalAmount"]):C2}";
                    txtNotes.Text = saleData.Rows[0]["Notes"]?.ToString() ?? "";
                }

                query = @"SELECT fs.Name as ServiceName, si.Quantity, si.UnitPrice, si.TotalPrice
                         FROM SaleItems si
                         INNER JOIN FinancialServices fs ON si.ServiceId = fs.Id
                         WHERE si.SaleId = @SaleId
                         ORDER BY fs.Name";
                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@SaleId", saleId));
                dataGridView.Columns["ServiceName"].HeaderText = "Услуга";
                dataGridView.Columns["Quantity"].HeaderText = "Количество";
                dataGridView.Columns["UnitPrice"].HeaderText = "Цена за единицу";
                dataGridView.Columns["UnitPrice"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["TotalPrice"].HeaderText = "Итого";
                dataGridView.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

