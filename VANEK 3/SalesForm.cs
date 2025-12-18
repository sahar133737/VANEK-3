using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SalesForm : Form
    {
        public SalesForm()
        {
            InitializeComponent();
            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                string query = @"SELECT s.Id, s.SaleDate, s.CustomerName, s.TotalAmount, s.Notes,
                                (SELECT COUNT(*) FROM SaleItems WHERE SaleId = s.Id) as ItemsCount
                                FROM Sales s WHERE 1=1";
                var parameters = new System.Collections.Generic.List<SqlParameter>();

                // Фильтр по дате (от)
                query += " AND CAST(s.SaleDate AS DATE) >= @DateFrom";
                parameters.Add(new SqlParameter("@DateFrom", dtpFilterDateFrom.Value.Date));

                // Фильтр по дате (до)
                query += " AND CAST(s.SaleDate AS DATE) <= @DateTo";
                parameters.Add(new SqlParameter("@DateTo", dtpFilterDateTo.Value.Date.AddDays(1).AddSeconds(-1)));

                // Фильтр по клиенту
                if (!string.IsNullOrWhiteSpace(txtFilterCustomer.Text))
                {
                    query += " AND s.CustomerName LIKE @CustomerName";
                    parameters.Add(new SqlParameter("@CustomerName", "%" + txtFilterCustomer.Text + "%"));
                }

                query += " ORDER BY s.SaleDate DESC";

                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                dataGridView.Columns["Id"].Visible = false;
                dataGridView.Columns["SaleDate"].HeaderText = "Дата продажи";
                dataGridView.Columns["SaleDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                dataGridView.Columns["CustomerName"].HeaderText = "Клиент";
                dataGridView.Columns["TotalAmount"].HeaderText = "Сумма";
                dataGridView.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["Notes"].HeaderText = "Примечания";
                dataGridView.Columns["ItemsCount"].HeaderText = "Количество позиций";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SaleEditForm form = new SaleEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSales();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продажу для редактирования!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            SaleEditForm form = new SaleEditForm(id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSales();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продажу для удаления!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную продажу? Все связанные позиции также будут удалены!", 
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    string query = "DELETE FROM Sales WHERE Id = @Id";
                    DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id));
                    MessageBox.Show("Продажа успешно удалена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продажу для просмотра!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            SaleDetailsForm form = new SaleDetailsForm(id);
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dtpFilterDateFrom.Value = System.DateTime.Now.AddMonths(-1);
            dtpFilterDateTo.Value = System.DateTime.Now;
            txtFilterCustomer.Clear();
            LoadSales();
        }
    }
}

