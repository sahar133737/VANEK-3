using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class FinancialServicesForm : BaseForm
    {
        private DataGridView dataGridView;
        private ComboBox cmbSearchCategory;
        private TextBox txtSearchName;

        public FinancialServicesForm()
        {
            InitializeComponent();
            FormTitle = "Финансовые услуги";
            HelpText = HelpTexts.FinancialServicesForm;
            LoadCategories();
            LoadServices();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "📊  Финансовые услуги";
            this.ClientSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 500);
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.WindowState = FormWindowState.Maximized;

            // Панель фильтров
            Panel pnlFilters = new Panel();
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 80;
            pnlFilters.BackColor = Color.White;
            pnlFilters.Padding = new Padding(15);
            this.Controls.Add(pnlFilters);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "📊  Каталог финансовых услуг";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(15, 10);
            lblTitle.AutoSize = true;
            pnlFilters.Controls.Add(lblTitle);

            // Фильтр по наименованию
            Label lblSearchName = new Label();
            lblSearchName.Text = "Наименование:";
            lblSearchName.Font = new Font("Segoe UI", 9F);
            lblSearchName.Location = new Point(15, 45);
            lblSearchName.AutoSize = true;
            pnlFilters.Controls.Add(lblSearchName);

            txtSearchName = new TextBox();
            txtSearchName.Font = new Font("Segoe UI", 9F);
            txtSearchName.Location = new Point(110, 42);
            txtSearchName.Size = new Size(200, 25);
            txtSearchName.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadServices(); };
            pnlFilters.Controls.Add(txtSearchName);

            // Фильтр по категории
            Label lblSearchCategory = new Label();
            lblSearchCategory.Text = "Категория:";
            lblSearchCategory.Font = new Font("Segoe UI", 9F);
            lblSearchCategory.Location = new Point(330, 45);
            lblSearchCategory.AutoSize = true;
            pnlFilters.Controls.Add(lblSearchCategory);

            cmbSearchCategory = new ComboBox();
            cmbSearchCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSearchCategory.Font = new Font("Segoe UI", 9F);
            cmbSearchCategory.Location = new Point(410, 42);
            cmbSearchCategory.Size = new Size(180, 25);
            cmbSearchCategory.SelectedIndexChanged += (s, e) => LoadServices();
            pnlFilters.Controls.Add(cmbSearchCategory);

            // Кнопки поиска
            Button btnSearch = new Button();
            btnSearch.Text = "🔍  Поиск";
            btnSearch.Size = new Size(100, 28);
            btnSearch.Location = new Point(610, 40);
            btnSearch.BackColor = Color.FromArgb(0, 120, 215);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Font = new Font("Segoe UI", 9F);
            btnSearch.Click += (s, e) => LoadServices();
            pnlFilters.Controls.Add(btnSearch);

            Button btnClearSearch = new Button();
            btnClearSearch.Text = "✖  Сброс";
            btnClearSearch.Size = new Size(100, 28);
            btnClearSearch.Location = new Point(720, 40);
            btnClearSearch.BackColor = Color.FromArgb(158, 158, 158);
            btnClearSearch.ForeColor = Color.White;
            btnClearSearch.FlatStyle = FlatStyle.Flat;
            btnClearSearch.FlatAppearance.BorderSize = 0;
            btnClearSearch.Cursor = Cursors.Hand;
            btnClearSearch.Font = new Font("Segoe UI", 9F);
            btnClearSearch.Click += btnClearSearch_Click;
            pnlFilters.Controls.Add(btnClearSearch);

            // Таблица данных
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(15, 95);
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersHeight = 35;
            dataGridView.RowTemplate.Height = 28;
            dataGridView.DoubleClick += (s, e) => btnEdit_Click(s, e);
            this.Controls.Add(dataGridView);

            // Панель кнопок
            Panel pnlButtons = new Panel();
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 60;
            pnlButtons.BackColor = Color.White;
            this.Controls.Add(pnlButtons);

            int btnX = 15;
            
            Button btnAdd = CreateActionButton("➕  Добавить", btnX, 12, ButtonColors.Add);
            btnAdd.Click += btnAdd_Click;
            btnAdd.TabIndex = 0;
            pnlButtons.Controls.Add(btnAdd);
            btnX += 130;

            Button btnEdit = CreateActionButton("✏️  Редактировать", btnX, 12, ButtonColors.Edit);
            btnEdit.Click += btnEdit_Click;
            btnEdit.TabIndex = 1;
            pnlButtons.Controls.Add(btnEdit);
            btnX += 150;

            Button btnDelete = CreateActionButton("🗑️  Удалить", btnX, 12, ButtonColors.Delete);
            btnDelete.Click += btnDelete_Click;
            btnDelete.TabIndex = 2;
            pnlButtons.Controls.Add(btnDelete);
            btnX += 120;

            Button btnRefresh = CreateActionButton("🔄  Обновить", btnX, 12, ButtonColors.Refresh);
            btnRefresh.Click += (s, e) => { LoadCategories(); LoadServices(); };
            btnRefresh.TabIndex = 3;
            pnlButtons.Controls.Add(btnRefresh);

            Button btnClose = CreateCloseButton(0, 12, 100, 35);
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.TabIndex = 4;
            btnClose.Click += (s, e) => this.Close();
            pnlButtons.Controls.Add(btnClose);

            // Корректировка позиции кнопки закрытия
            pnlButtons.Resize += (s, e) => {
                if (btnClose != null)
                    btnClose.Location = new Point(pnlButtons.Width - 115, 12);
            };

            // Корректируем размер DataGridView
            this.Resize += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 30, 
                    this.ClientSize.Height - pnlFilters.Height - pnlButtons.Height - 15);
            };

            this.Load += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 30, 
                    this.ClientSize.Height - pnlFilters.Height - pnlButtons.Height - 15);
            };

            this.ResumeLayout(false);
        }

        private Button CreateActionButton(string text, int x, Color backColor)
        {
            return CreateActionButton(text, x, 12, backColor);
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
                
                if (dataGridView.Columns["Id"] != null)
                    dataGridView.Columns["Id"].Visible = false;
                if (dataGridView.Columns["Name"] != null)
                    dataGridView.Columns["Name"].HeaderText = "Наименование";
                if (dataGridView.Columns["Category"] != null)
                    dataGridView.Columns["Category"].HeaderText = "Категория";
                if (dataGridView.Columns["Price"] != null)
                {
                    dataGridView.Columns["Price"].HeaderText = "Цена";
                    dataGridView.Columns["Price"].DefaultCellStyle.Format = "N2";
                }
                if (dataGridView.Columns["Description"] != null)
                    dataGridView.Columns["Description"].HeaderText = "Описание";
                if (dataGridView.Columns["CreatedDate"] != null)
                {
                    dataGridView.Columns["CreatedDate"].HeaderText = "Дата создания";
                    dataGridView.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
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

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchName.Clear();
            cmbSearchCategory.SelectedIndex = 0;
            LoadServices();
        }
    }
}
