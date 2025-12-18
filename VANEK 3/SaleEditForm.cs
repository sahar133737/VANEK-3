using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SaleEditForm : Form
    {
        private int? saleId;
        private DataTable saleItems;

        public SaleEditForm(int? id = null)
        {
            InitializeComponent();
            saleId = id;
            saleItems = new DataTable();
            saleItems.Columns.Add("ServiceId", typeof(int));
            saleItems.Columns.Add("ServiceName", typeof(string));
            saleItems.Columns.Add("Quantity", typeof(int));
            saleItems.Columns.Add("UnitPrice", typeof(decimal));
            saleItems.Columns.Add("TotalPrice", typeof(decimal));
            dataGridViewItems.DataSource = saleItems;
            dataGridViewItems.Columns["ServiceId"].Visible = false;
            dataGridViewItems.Columns["ServiceName"].HeaderText = "Услуга";
            dataGridViewItems.Columns["Quantity"].HeaderText = "Количество";
            dataGridViewItems.Columns["UnitPrice"].HeaderText = "Цена за единицу";
            dataGridViewItems.Columns["UnitPrice"].DefaultCellStyle.Format = "C2";
            dataGridViewItems.Columns["TotalPrice"].HeaderText = "Итого";
            dataGridViewItems.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
            LoadServices();
            
            if (id.HasValue)
            {
                LoadSale(id.Value);
                this.Text = "Редактирование продажи";
            }
            else
            {
                dtpSaleDate.Value = DateTime.Now;
                this.Text = "Добавление продажи";
            }
        }

        private void LoadServices()
        {
            try
            {
                string query = "SELECT Id, Name, Price FROM FinancialServices ORDER BY Name";
                var dt = DatabaseHelper.ExecuteQuery(query);
                cmbService.DataSource = dt;
                cmbService.DisplayMember = "Name";
                cmbService.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSale(int id)
        {
            try
            {
                string query = "SELECT SaleDate, CustomerName, TotalAmount, Notes FROM Sales WHERE Id = @Id";
                var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    dtpSaleDate.Value = Convert.ToDateTime(dt.Rows[0]["SaleDate"]);
                    txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                }

                query = @"SELECT si.ServiceId, fs.Name as ServiceName, si.Quantity, si.UnitPrice, si.TotalPrice
                         FROM SaleItems si
                         INNER JOIN FinancialServices fs ON si.ServiceId = fs.Id
                         WHERE si.SaleId = @SaleId";
                var items = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@SaleId", id));
                saleItems.Clear();
                foreach (DataRow row in items.Rows)
                {
                    saleItems.Rows.Add(row["ServiceId"], row["ServiceName"], row["Quantity"], 
                        row["UnitPrice"], row["TotalPrice"]);
                }
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cmbService.SelectedValue == null)
            {
                MessageBox.Show("Выберите услугу!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            int serviceId = Convert.ToInt32(cmbService.SelectedValue);
            string serviceName = cmbService.Text;
            decimal unitPrice = GetServicePrice(serviceId);
            decimal totalPrice = unitPrice * quantity;

            saleItems.Rows.Add(serviceId, serviceName, quantity, unitPrice, totalPrice);
            UpdateTotal();
            txtQuantity.Clear();
        }

        private decimal GetServicePrice(int serviceId)
        {
            try
            {
                string query = "SELECT Price FROM FinancialServices WHERE Id = @Id";
                var result = DatabaseHelper.ExecuteScalar(query, new SqlParameter("@Id", serviceId));
                return Convert.ToDecimal(result);
            }
            catch
            {
                return 0;
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridViewItems.SelectedRows)
                {
                    dataGridViewItems.Rows.RemoveAt(row.Index);
                }
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in saleItems.Rows)
            {
                total += Convert.ToDecimal(row["TotalPrice"]);
            }
            lblTotal.Text = $"Итого: {total:C2}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Введите имя клиента!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerName.Focus();
                return;
            }

            if (saleItems.Rows.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну позицию!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal totalAmount = saleItems.AsEnumerable().Sum(r => Convert.ToDecimal(r["TotalPrice"]));

                if (saleId.HasValue)
                {
                    string query = @"UPDATE Sales SET SaleDate = @SaleDate, CustomerName = @CustomerName, 
                                   TotalAmount = @TotalAmount, Notes = @Notes WHERE Id = @Id";
                    DatabaseHelper.ExecuteNonQuery(query,
                        new SqlParameter("@SaleDate", dtpSaleDate.Value),
                        new SqlParameter("@CustomerName", txtCustomerName.Text),
                        new SqlParameter("@TotalAmount", totalAmount),
                        new SqlParameter("@Notes", txtNotes.Text ?? (object)DBNull.Value),
                        new SqlParameter("@Id", saleId.Value));

                    DatabaseHelper.ExecuteNonQuery("DELETE FROM SaleItems WHERE SaleId = @SaleId",
                        new SqlParameter("@SaleId", saleId.Value));
                }
                else
                {
                    string query = @"INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes) 
                                   OUTPUT INSERTED.Id
                                   VALUES (@SaleDate, @CustomerName, @TotalAmount, @Notes)";
                    saleId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query,
                        new SqlParameter("@SaleDate", dtpSaleDate.Value),
                        new SqlParameter("@CustomerName", txtCustomerName.Text),
                        new SqlParameter("@TotalAmount", totalAmount),
                        new SqlParameter("@Notes", txtNotes.Text ?? (object)DBNull.Value)));
                }

                foreach (DataRow row in saleItems.Rows)
                {
                    string query = @"INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice) 
                                   VALUES (@SaleId, @ServiceId, @Quantity, @UnitPrice, @TotalPrice)";
                    DatabaseHelper.ExecuteNonQuery(query,
                        new SqlParameter("@SaleId", saleId.Value),
                        new SqlParameter("@ServiceId", row["ServiceId"]),
                        new SqlParameter("@Quantity", row["Quantity"]),
                        new SqlParameter("@UnitPrice", row["UnitPrice"]),
                        new SqlParameter("@TotalPrice", row["TotalPrice"]));
                }

                MessageBox.Show("Продажа успешно сохранена!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

