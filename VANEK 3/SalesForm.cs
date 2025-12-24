using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SalesForm : BaseForm
    {
        private DataGridView dataGridView;
        private DateTimePicker dtpFilterDateFrom;
        private DateTimePicker dtpFilterDateTo;
        private ComboBox cmbFilterCustomer;

        public SalesForm()
        {
            InitializeComponent();
            FormTitle = "Продажи";
            HelpText = HelpTexts.SalesForm;
            LoadCustomers();
            LoadSales();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "💰  Продажи";
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
            pnlFilters.Height = 100;
            pnlFilters.BackColor = Color.White;
            pnlFilters.Padding = new Padding(15);
            this.Controls.Add(pnlFilters);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "💰  Учет продаж";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(15, 10);
            lblTitle.AutoSize = true;
            pnlFilters.Controls.Add(lblTitle);

            // Фильтр по дате с
            Label lblDateFrom = new Label();
            lblDateFrom.Text = "Дата с:";
            lblDateFrom.Font = new Font("Segoe UI", 9F);
            lblDateFrom.Location = new Point(15, 45);
            lblDateFrom.AutoSize = true;
            pnlFilters.Controls.Add(lblDateFrom);

            dtpFilterDateFrom = new DateTimePicker();
            dtpFilterDateFrom.Format = DateTimePickerFormat.Short;
            dtpFilterDateFrom.Font = new Font("Segoe UI", 9F);
            dtpFilterDateFrom.Location = new Point(70, 42);
            dtpFilterDateFrom.Size = new Size(120, 25);
            dtpFilterDateFrom.Value = DateTime.Now.AddMonths(-1);
            pnlFilters.Controls.Add(dtpFilterDateFrom);

            // Фильтр по дате по
            Label lblDateTo = new Label();
            lblDateTo.Text = "по:";
            lblDateTo.Font = new Font("Segoe UI", 9F);
            lblDateTo.Location = new Point(200, 45);
            lblDateTo.AutoSize = true;
            pnlFilters.Controls.Add(lblDateTo);

            dtpFilterDateTo = new DateTimePicker();
            dtpFilterDateTo.Format = DateTimePickerFormat.Short;
            dtpFilterDateTo.Font = new Font("Segoe UI", 9F);
            dtpFilterDateTo.Location = new Point(225, 42);
            dtpFilterDateTo.Size = new Size(120, 25);
            dtpFilterDateTo.Value = DateTime.Now;
            pnlFilters.Controls.Add(dtpFilterDateTo);

            // Фильтр по клиенту
            Label lblCustomer = new Label();
            lblCustomer.Text = "Клиент:";
            lblCustomer.Font = new Font("Segoe UI", 9F);
            lblCustomer.Location = new Point(365, 45);
            lblCustomer.AutoSize = true;
            pnlFilters.Controls.Add(lblCustomer);

            cmbFilterCustomer = new ComboBox();
            cmbFilterCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterCustomer.Font = new Font("Segoe UI", 9F);
            cmbFilterCustomer.Location = new Point(420, 42);
            cmbFilterCustomer.Size = new Size(200, 25);
            cmbFilterCustomer.SelectedIndexChanged += (s, e) => LoadSales();
            pnlFilters.Controls.Add(cmbFilterCustomer);

            // Кнопки поиска
            Button btnFilter = new Button();
            btnFilter.Text = "🔍  Применить";
            btnFilter.Size = new Size(120, 28);
            btnFilter.Location = new Point(640, 40);
            btnFilter.BackColor = Color.FromArgb(0, 120, 215);
            btnFilter.ForeColor = Color.White;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Cursor = Cursors.Hand;
            btnFilter.Font = new Font("Segoe UI", 9F);
            btnFilter.Click += (s, e) => LoadSales();
            pnlFilters.Controls.Add(btnFilter);

            Button btnClearFilter = new Button();
            btnClearFilter.Text = "✖  Сброс";
            btnClearFilter.Size = new Size(100, 28);
            btnClearFilter.Location = new Point(770, 40);
            btnClearFilter.BackColor = Color.FromArgb(158, 158, 158);
            btnClearFilter.ForeColor = Color.White;
            btnClearFilter.FlatStyle = FlatStyle.Flat;
            btnClearFilter.FlatAppearance.BorderSize = 0;
            btnClearFilter.Cursor = Cursors.Hand;
            btnClearFilter.Font = new Font("Segoe UI", 9F);
            btnClearFilter.Click += btnClearFilter_Click;
            pnlFilters.Controls.Add(btnClearFilter);

            // Таблица данных
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(15, 115);
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
            dataGridView.DoubleClick += (s, e) => btnViewDetails_Click(s, e);
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

            Button btnViewDetails = CreateActionButton("👁️  Детали", btnX, 12, ButtonColors.View);
            btnViewDetails.Click += btnViewDetails_Click;
            btnViewDetails.TabIndex = 3;
            pnlButtons.Controls.Add(btnViewDetails);
            btnX += 115;

            Button btnRefresh = CreateActionButton("🔄  Обновить", btnX, 12, ButtonColors.Refresh);
            btnRefresh.Click += (s, e) => LoadSales();
            btnRefresh.TabIndex = 4;
            pnlButtons.Controls.Add(btnRefresh);

            Button btnClose = CreateCloseButton(0, 12, 100, 35);
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.TabIndex = 5;
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

        private void LoadCustomers()
        {
            try
            {
                string query = "SELECT DISTINCT CustomerName FROM Sales ORDER BY CustomerName";
                var dt = DatabaseHelper.ExecuteQuery(query);
                cmbFilterCustomer.Items.Clear();
                cmbFilterCustomer.Items.Add("Все клиенты");
                foreach (DataRow row in dt.Rows)
                {
                    cmbFilterCustomer.Items.Add(row["CustomerName"].ToString());
                }
                cmbFilterCustomer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                if (cmbFilterCustomer.SelectedIndex > 0 && cmbFilterCustomer.SelectedItem != null)
                {
                    query += " AND s.CustomerName = @CustomerName";
                    parameters.Add(new SqlParameter("@CustomerName", cmbFilterCustomer.SelectedItem.ToString()));
                }

                query += " ORDER BY s.SaleDate DESC";

                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                
                if (dataGridView.Columns["Id"] != null)
                    dataGridView.Columns["Id"].Visible = false;
                if (dataGridView.Columns["SaleDate"] != null)
                {
                    dataGridView.Columns["SaleDate"].HeaderText = "Дата продажи";
                    dataGridView.Columns["SaleDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
                if (dataGridView.Columns["CustomerName"] != null)
                    dataGridView.Columns["CustomerName"].HeaderText = "Клиент";
                if (dataGridView.Columns["TotalAmount"] != null)
                {
                    dataGridView.Columns["TotalAmount"].HeaderText = "Сумма";
                    dataGridView.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
                }
                if (dataGridView.Columns["Notes"] != null)
                    dataGridView.Columns["Notes"].HeaderText = "Примечания";
                if (dataGridView.Columns["ItemsCount"] != null)
                    dataGridView.Columns["ItemsCount"].HeaderText = "Позиций";
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
                LoadCustomers();
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
                LoadCustomers();
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

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dtpFilterDateFrom.Value = DateTime.Now.AddMonths(-1);
            dtpFilterDateTo.Value = DateTime.Now;
            cmbFilterCustomer.SelectedIndex = 0;
            LoadSales();
        }
    }
}
