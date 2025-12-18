using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class FinancialServicesForm : Form
    {
        public FinancialServicesForm()
        {
            InitializeComponent();
            LoadCategories();
            LoadServices();
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT DISTINCT Category FROM FinancialServices WHERE Category IS NOT NULL AND Category != '' ORDER BY Category";
                var dt = DatabaseHelper.ExecuteQuery(query);
                cmbSearchCategory.Items.Clear();
                cmbSearchCategory.Items.Add("Все категории");
                foreach (DataRow row in dt.Rows)
                {
                    cmbSearchCategory.Items.Add(row["Category"].ToString());
                }
                cmbSearchCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadServices()
        {
            try
            {
                string query = "SELECT Id, Name, Category, Price, Description, CreatedDate FROM FinancialServices WHERE 1=1";
                var parameters = new System.Collections.Generic.List<SqlParameter>();

                // Фильтр по наименованию
                if (!string.IsNullOrWhiteSpace(txtSearchName.Text))
                {
                    query += " AND Name LIKE @Name";
                    parameters.Add(new SqlParameter("@Name", "%" + txtSearchName.Text + "%"));
                }

                // Фильтр по категории
                if (cmbSearchCategory.SelectedIndex > 0 && cmbSearchCategory.SelectedItem != null)
                {
                    query += " AND Category = @Category";
                    parameters.Add(new SqlParameter("@Category", cmbSearchCategory.SelectedItem.ToString()));
                }

                query += " ORDER BY Name";

                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                dataGridView.Columns["Id"].Visible = false;
                dataGridView.Columns["Name"].HeaderText = "Наименование";
                dataGridView.Columns["Category"].HeaderText = "Категория";
                dataGridView.Columns["Price"].HeaderText = "Цена";
                dataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
                dataGridView.Columns["Description"].HeaderText = "Описание";
                dataGridView.Columns["CreatedDate"].HeaderText = "Дата создания";
                dataGridView.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FinancialServiceEditForm form = new FinancialServiceEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
                LoadServices();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите услугу для редактирования!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            FinancialServiceEditForm form = new FinancialServiceEditForm(id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCategories();
                LoadServices();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите услугу для удаления!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную услугу?", "Подтверждение", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
                    string query = "DELETE FROM FinancialServices WHERE Id = @Id";
                    DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id));
                    MessageBox.Show("Услуга успешно удалена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadServices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            LoadServices();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchName.Clear();
            cmbSearchCategory.SelectedIndex = 0;
            LoadServices();
        }
    }
}

