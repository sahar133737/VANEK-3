using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class FinancialServiceEditForm : BaseForm
    {
        private int? serviceId;
        private TextBox txtName;
        private ComboBox cmbCategory;
        private TextBox txtPrice;
        private TextBox txtDescription;

        public FinancialServiceEditForm(int? id = null)
        {
            InitializeComponent();
            FormTitle = id.HasValue ? "Редактирование услуги" : "Новая услуга";
            HelpText = HelpTexts.FinancialServicesForm;
            
            serviceId = id;
            LoadCategories();
            
            if (id.HasValue)
            {
                LoadService(id.Value);
                this.Text = "✏️  Редактирование услуги";
            }
            else
            {
                this.Text = "➕  Добавление услуги";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.ClientSize = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Панель с полями
            Panel pnlFields = new Panel();
            pnlFields.Location = new Point(25, 20);
            pnlFields.Size = new Size(450, 320);
            pnlFields.BackColor = Color.White;
            this.Controls.Add(pnlFields);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "📋  Информация об услуге";
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            pnlFields.Controls.Add(lblTitle);

            // Название
            Label lblName = new Label();
            lblName.Text = "Название:";
            lblName.Font = new Font("Segoe UI", 9F);
            lblName.Location = new Point(20, 55);
            lblName.AutoSize = true;
            pnlFields.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Font = new Font("Segoe UI", 9F);
            txtName.Location = new Point(20, 75);
            txtName.Size = new Size(410, 25);
            pnlFields.Controls.Add(txtName);

            // Категория
            Label lblCategory = new Label();
            lblCategory.Text = "Категория:";
            lblCategory.Font = new Font("Segoe UI", 9F);
            lblCategory.Location = new Point(20, 110);
            lblCategory.AutoSize = true;
            pnlFields.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Font = new Font("Segoe UI", 9F);
            cmbCategory.Location = new Point(20, 130);
            cmbCategory.Size = new Size(200, 25);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDown;
            pnlFields.Controls.Add(cmbCategory);

            // Цена
            Label lblPrice = new Label();
            lblPrice.Text = "Цена (₽):";
            lblPrice.Font = new Font("Segoe UI", 9F);
            lblPrice.Location = new Point(240, 110);
            lblPrice.AutoSize = true;
            pnlFields.Controls.Add(lblPrice);

            txtPrice = new TextBox();
            txtPrice.Font = new Font("Segoe UI", 9F);
            txtPrice.Location = new Point(240, 130);
            txtPrice.Size = new Size(190, 25);
            pnlFields.Controls.Add(txtPrice);

            // Описание
            Label lblDescription = new Label();
            lblDescription.Text = "Описание:";
            lblDescription.Font = new Font("Segoe UI", 9F);
            lblDescription.Location = new Point(20, 165);
            lblDescription.AutoSize = true;
            pnlFields.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 9F);
            txtDescription.Location = new Point(20, 185);
            txtDescription.Size = new Size(410, 80);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            pnlFields.Controls.Add(txtDescription);

            // Кнопки
            Button btnSave = new Button();
            btnSave.Text = "💾 Сохранить";
            btnSave.Size = new Size(120, 35);
            btnSave.Location = new Point(200, 280);
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.Click += btnSave_Click;
            pnlFields.Controls.Add(btnSave);

            Button btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(100, 35);
            btnCancel.Location = new Point(330, 280);
            btnCancel.BackColor = Color.FromArgb(97, 97, 97);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.Click += (s, e) => this.Close();
            pnlFields.Controls.Add(btnCancel);

            // Подсказка F1
            Label lblHint = new Label();
            lblHint.Text = "💡 Нажмите F1 для справки";
            lblHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblHint.ForeColor = Color.FromArgb(150, 150, 155);
            lblHint.Location = new Point(30, 355);
            lblHint.AutoSize = true;
            this.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT DISTINCT Category FROM FinancialServices WHERE Category IS NOT NULL AND Category != '' ORDER BY Category";
                var dt = DatabaseHelper.ExecuteQuery(query);
                cmbCategory.Items.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    cmbCategory.Items.Add(row["Category"].ToString());
                }
                
                // Добавляем стандартные категории если их нет
                string[] defaultCategories = { "Консультации", "Бухгалтерия", "Налоговые услуги", "Аудит", "Регистрация", "Юридические услуги", "Кадры", "Ликвидация" };
                foreach (string cat in defaultCategories)
                {
                    if (!cmbCategory.Items.Contains(cat))
                        cmbCategory.Items.Add(cat);
                }
            }
            catch { }
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
                    cmbCategory.Text = dt.Rows[0]["Category"].ToString();
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

            if (!decimal.TryParse(txtPrice.Text.Replace(",", ".").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out decimal price) || price <= 0)
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
                        new SqlParameter("@Category", cmbCategory.Text ?? (object)DBNull.Value),
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
                        new SqlParameter("@Category", cmbCategory.Text ?? (object)DBNull.Value),
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
    }
}
