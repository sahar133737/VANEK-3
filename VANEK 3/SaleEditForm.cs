using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SaleEditForm : BaseForm
    {
        private int? saleId;
        private DataTable saleItems;
        private DateTimePicker dtpSaleDate;
        private TextBox txtCustomerName;
        private TextBox txtNotes;
        private ComboBox cmbService;
        private TextBox txtQuantity;
        private DataGridView dataGridViewItems;
        private Label lblTotal;

        public SaleEditForm(int? id = null)
        {
            InitializeComponent();
            FormTitle = id.HasValue ? "Редактирование продажи" : "Новая продажа";
            HelpText = HelpTexts.SaleEditForm;
            
            saleId = id;
            saleItems = new DataTable();
            saleItems.Columns.Add("ServiceId", typeof(int));
            saleItems.Columns.Add("ServiceName", typeof(string));
            saleItems.Columns.Add("Quantity", typeof(int));
            saleItems.Columns.Add("UnitPrice", typeof(decimal));
            saleItems.Columns.Add("TotalPrice", typeof(decimal));
            dataGridViewItems.DataSource = saleItems;
            
            ConfigureItemsGrid();
            LoadServices();
            
            if (id.HasValue)
            {
                LoadSale(id.Value);
                this.Text = "✏️  Редактирование продажи";
            }
            else
            {
                dtpSaleDate.Value = DateTime.Now;
                this.Text = "➕  Новая продажа";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.ClientSize = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(650, 550);
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Панель информации о продаже
            Panel pnlInfo = new Panel();
            pnlInfo.Dock = DockStyle.Top;
            pnlInfo.Height = 180;
            pnlInfo.BackColor = Color.White;
            pnlInfo.Padding = new Padding(20);
            this.Controls.Add(pnlInfo);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "📋  Информация о продаже";
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;
            pnlInfo.Controls.Add(lblTitle);

            // Дата
            Label lblSaleDate = new Label();
            lblSaleDate.Text = "Дата:";
            lblSaleDate.Font = new Font("Segoe UI", 9F);
            lblSaleDate.Location = new Point(20, 45);
            lblSaleDate.AutoSize = true;
            pnlInfo.Controls.Add(lblSaleDate);

            dtpSaleDate = new DateTimePicker();
            dtpSaleDate.Font = new Font("Segoe UI", 9F);
            dtpSaleDate.Location = new Point(120, 42);
            dtpSaleDate.Size = new Size(200, 25);
            pnlInfo.Controls.Add(dtpSaleDate);

            // Клиент
            Label lblCustomerName = new Label();
            lblCustomerName.Text = "Клиент:";
            lblCustomerName.Font = new Font("Segoe UI", 9F);
            lblCustomerName.Location = new Point(20, 80);
            lblCustomerName.AutoSize = true;
            pnlInfo.Controls.Add(lblCustomerName);

            txtCustomerName = new TextBox();
            txtCustomerName.Font = new Font("Segoe UI", 9F);
            txtCustomerName.Location = new Point(120, 77);
            txtCustomerName.Size = new Size(400, 25);
            pnlInfo.Controls.Add(txtCustomerName);

            // Примечания
            Label lblNotes = new Label();
            lblNotes.Text = "Примечания:";
            lblNotes.Font = new Font("Segoe UI", 9F);
            lblNotes.Location = new Point(20, 115);
            lblNotes.AutoSize = true;
            pnlInfo.Controls.Add(lblNotes);

            txtNotes = new TextBox();
            txtNotes.Font = new Font("Segoe UI", 9F);
            txtNotes.Location = new Point(120, 112);
            txtNotes.Size = new Size(400, 50);
            txtNotes.Multiline = true;
            pnlInfo.Controls.Add(txtNotes);

            // Панель добавления услуги
            Panel pnlAddService = new Panel();
            pnlAddService.Dock = DockStyle.Top;
            pnlAddService.Height = 60;
            pnlAddService.BackColor = Color.FromArgb(250, 250, 252);
            pnlAddService.Padding = new Padding(20, 10, 20, 10);
            this.Controls.Add(pnlAddService);

            Label lblService = new Label();
            lblService.Text = "Услуга:";
            lblService.Font = new Font("Segoe UI", 9F);
            lblService.Location = new Point(20, 18);
            lblService.AutoSize = true;
            pnlAddService.Controls.Add(lblService);

            cmbService = new ComboBox();
            cmbService.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbService.Font = new Font("Segoe UI", 9F);
            cmbService.Location = new Point(75, 15);
            cmbService.Size = new Size(250, 25);
            pnlAddService.Controls.Add(cmbService);

            Label lblQuantity = new Label();
            lblQuantity.Text = "Кол-во:";
            lblQuantity.Font = new Font("Segoe UI", 9F);
            lblQuantity.Location = new Point(340, 18);
            lblQuantity.AutoSize = true;
            pnlAddService.Controls.Add(lblQuantity);

            txtQuantity = new TextBox();
            txtQuantity.Font = new Font("Segoe UI", 9F);
            txtQuantity.Location = new Point(395, 15);
            txtQuantity.Size = new Size(60, 25);
            txtQuantity.Text = "1";
            pnlAddService.Controls.Add(txtQuantity);

            Button btnAddItem = new Button();
            btnAddItem.Text = "➕ Добавить";
            btnAddItem.Size = new Size(110, 30);
            btnAddItem.Location = new Point(470, 12);
            btnAddItem.BackColor = Color.FromArgb(76, 175, 80);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.Cursor = Cursors.Hand;
            btnAddItem.Font = new Font("Segoe UI", 9F);
            btnAddItem.Click += btnAddItem_Click;
            pnlAddService.Controls.Add(btnAddItem);

            // Таблица услуг
            dataGridViewItems = new DataGridView();
            dataGridViewItems.Location = new Point(20, 255);
            dataGridViewItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewItems.AllowUserToAddRows = false;
            dataGridViewItems.ReadOnly = true;
            dataGridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewItems.MultiSelect = true;
            dataGridViewItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewItems.BackgroundColor = Color.White;
            dataGridViewItems.BorderStyle = BorderStyle.None;
            dataGridViewItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
            dataGridViewItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dataGridViewItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewItems.EnableHeadersVisualStyles = false;
            dataGridViewItems.ColumnHeadersHeight = 30;
            dataGridViewItems.RowTemplate.Height = 25;
            this.Controls.Add(dataGridViewItems);

            // Панель итогов и кнопок
            Panel pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 70;
            pnlBottom.BackColor = Color.White;
            this.Controls.Add(pnlBottom);

            lblTotal = new Label();
            lblTotal.Text = "💰 Итого: 0,00 ₽";
            lblTotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(0, 120, 215);
            lblTotal.Location = new Point(20, 20);
            lblTotal.AutoSize = true;
            pnlBottom.Controls.Add(lblTotal);

            Button btnRemoveItem = new Button();
            btnRemoveItem.Text = "🗑️ Удалить позицию";
            btnRemoveItem.Size = new Size(150, 35);
            btnRemoveItem.Location = new Point(250, 17);
            btnRemoveItem.BackColor = Color.FromArgb(244, 67, 54);
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.Cursor = Cursors.Hand;
            btnRemoveItem.Font = new Font("Segoe UI", 9F);
            btnRemoveItem.Click += btnRemoveItem_Click;
            pnlBottom.Controls.Add(btnRemoveItem);

            Button btnSave = new Button();
            btnSave.Text = "💾 Сохранить";
            btnSave.Size = new Size(120, 35);
            btnSave.Location = new Point(440, 17);
            btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.Click += btnSave_Click;
            pnlBottom.Controls.Add(btnSave);

            Button btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(100, 35);
            btnCancel.Location = new Point(570, 17);
            btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnCancel.BackColor = Color.FromArgb(97, 97, 97);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnCancel);

            // Корректируем размер DataGridView
            this.Resize += (s, e) => {
                dataGridViewItems.Size = new Size(this.ClientSize.Width - 40, 
                    this.ClientSize.Height - 255 - pnlBottom.Height - 15);
            };

            this.Load += (s, e) => {
                dataGridViewItems.Size = new Size(this.ClientSize.Width - 40, 
                    this.ClientSize.Height - 255 - pnlBottom.Height - 15);
            };

            this.ResumeLayout(false);
        }

        private void ConfigureItemsGrid()
        {
            if (dataGridViewItems.Columns["ServiceId"] != null)
                dataGridViewItems.Columns["ServiceId"].Visible = false;
            if (dataGridViewItems.Columns["ServiceName"] != null)
                dataGridViewItems.Columns["ServiceName"].HeaderText = "Услуга";
            if (dataGridViewItems.Columns["Quantity"] != null)
                dataGridViewItems.Columns["Quantity"].HeaderText = "Количество";
            if (dataGridViewItems.Columns["UnitPrice"] != null)
            {
                dataGridViewItems.Columns["UnitPrice"].HeaderText = "Цена за ед.";
                dataGridViewItems.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            }
            if (dataGridViewItems.Columns["TotalPrice"] != null)
            {
                dataGridViewItems.Columns["TotalPrice"].HeaderText = "Итого";
                dataGridViewItems.Columns["TotalPrice"].DefaultCellStyle.Format = "N2";
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
                ConfigureItemsGrid();
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
            ConfigureItemsGrid();
            UpdateTotal();
            txtQuantity.Text = "1";
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
                    if (!row.IsNewRow)
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
            lblTotal.Text = $"💰 Итого: {total:N2} ₽";
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
    }
}
