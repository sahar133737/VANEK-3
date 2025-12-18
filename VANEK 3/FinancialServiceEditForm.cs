using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class FinancialServiceEditForm : Form
    {
        private int? serviceId;

        public FinancialServiceEditForm(int? id = null)
        {
            InitializeComponent();
            serviceId = id;
            if (id.HasValue)
            {
                LoadService(id.Value);
                this.Text = "Редактирование услуги";
            }
            else
            {
                this.Text = "Добавление услуги";
            }
        }

        private void LoadService(int id)
        {
            try
            {
                string query = "SELECT Name, Category, Price, Description FROM FinancialServices WHERE Id = @Id";
                var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtCategory.Text = dt.Rows[0]["Category"].ToString();
                    txtPrice.Text = dt.Rows[0]["Price"].ToString();
                    txtDescription.Text = dt.Rows[0]["Description"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название услуги!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            try
            {
                if (serviceId.HasValue)
                {
                    string query = @"UPDATE FinancialServices SET Name = @Name, Category = @Category, 
                                   Price = @Price, Description = @Description WHERE Id = @Id";
                    DatabaseHelper.ExecuteNonQuery(query,
                        new SqlParameter("@Name", txtName.Text),
                        new SqlParameter("@Category", txtCategory.Text ?? (object)DBNull.Value),
                        new SqlParameter("@Price", price),
                        new SqlParameter("@Description", txtDescription.Text ?? (object)DBNull.Value),
                        new SqlParameter("@Id", serviceId.Value));
                    MessageBox.Show("Услуга успешно обновлена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string query = @"INSERT INTO FinancialServices (Name, Category, Price, Description, CreatedDate) 
                                   VALUES (@Name, @Category, @Price, @Description, GETDATE())";
                    DatabaseHelper.ExecuteNonQuery(query,
                        new SqlParameter("@Name", txtName.Text),
                        new SqlParameter("@Category", txtCategory.Text ?? (object)DBNull.Value),
                        new SqlParameter("@Price", price),
                        new SqlParameter("@Description", txtDescription.Text ?? (object)DBNull.Value));
                    MessageBox.Show("Услуга успешно добавлена!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

