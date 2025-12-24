using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SaleDetailsForm : BaseForm
    {
        private Label lblSaleDate;
        private Label lblCustomerName;
        private Label lblTotalAmount;
        private TextBox txtNotes;
        private DataGridView dataGridView;

        public SaleDetailsForm(int saleId)
        {
            InitializeComponent();
            FormTitle = "Детали продажи";
            HelpText = HelpTexts.SalesForm;
            LoadSaleDetails(saleId);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "👁️  Детали продажи";
            this.ClientSize = new Size(650, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(600, 500);
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Панель информации
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
            lblSaleDate = new Label();
            lblSaleDate.Font = new Font("Segoe UI", 10F);
            lblSaleDate.ForeColor = Color.FromArgb(45, 45, 48);
            lblSaleDate.Location = new Point(20, 45);
            lblSaleDate.AutoSize = true;
            pnlInfo.Controls.Add(lblSaleDate);

            // Клиент
            lblCustomerName = new Label();
            lblCustomerName.Font = new Font("Segoe UI", 10F);
            lblCustomerName.ForeColor = Color.FromArgb(45, 45, 48);
            lblCustomerName.Location = new Point(20, 70);
            lblCustomerName.AutoSize = true;
            pnlInfo.Controls.Add(lblCustomerName);

            // Итого
            lblTotalAmount = new Label();
            lblTotalAmount.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalAmount.ForeColor = Color.FromArgb(0, 120, 215);
            lblTotalAmount.Location = new Point(20, 95);
            lblTotalAmount.AutoSize = true;
            pnlInfo.Controls.Add(lblTotalAmount);

            // Примечания
            Label lblNotesTitle = new Label();
            lblNotesTitle.Text = "Примечания:";
            lblNotesTitle.Font = new Font("Segoe UI", 9F);
            lblNotesTitle.ForeColor = Color.FromArgb(100, 100, 105);
            lblNotesTitle.Location = new Point(20, 125);
            lblNotesTitle.AutoSize = true;
            pnlInfo.Controls.Add(lblNotesTitle);

            txtNotes = new TextBox();
            txtNotes.Font = new Font("Segoe UI", 9F);
            txtNotes.Location = new Point(20, 142);
            txtNotes.Size = new Size(590, 30);
            txtNotes.ReadOnly = true;
            txtNotes.BackColor = Color.FromArgb(250, 250, 252);
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            pnlInfo.Controls.Add(txtNotes);

            // Заголовок таблицы
            Label lblItems = new Label();
            lblItems.Text = "📦  Позиции продажи";
            lblItems.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblItems.ForeColor = Color.FromArgb(45, 45, 48);
            lblItems.Location = new Point(20, 195);
            lblItems.AutoSize = true;
            this.Controls.Add(lblItems);

            // Таблица
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(20, 220);
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
            this.Controls.Add(dataGridView);

            // Панель кнопок
            Panel pnlButtons = new Panel();
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 60;
            pnlButtons.BackColor = Color.White;
            this.Controls.Add(pnlButtons);

            Button btnClose = new Button();
            btnClose.Text = "Закрыть";
            btnClose.Size = new Size(100, 35);
            btnClose.Location = new Point(0, 12);
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.BackColor = Color.FromArgb(97, 97, 97);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Font = new Font("Segoe UI", 9F);
            btnClose.Click += (s, e) => this.Close();
            pnlButtons.Controls.Add(btnClose);

            pnlButtons.Resize += (s, e) => {
                btnClose.Location = new Point(pnlButtons.Width - 115, 12);
            };

            // Корректируем размер DataGridView
            this.Resize += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 40, 
                    this.ClientSize.Height - pnlInfo.Height - pnlButtons.Height - 45);
                txtNotes.Size = new Size(pnlInfo.Width - 40, 30);
            };

            this.Load += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 40, 
                    this.ClientSize.Height - pnlInfo.Height - pnlButtons.Height - 45);
            };

            this.ResumeLayout(false);
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
                    lblSaleDate.Text = $"📅  Дата: {Convert.ToDateTime(saleData.Rows[0]["SaleDate"]):dd.MM.yyyy HH:mm}";
                    lblCustomerName.Text = $"👤  Клиент: {saleData.Rows[0]["CustomerName"]}";
                    lblTotalAmount.Text = $"💰  Итого: {Convert.ToDecimal(saleData.Rows[0]["TotalAmount"]):N2} ₽";
                    txtNotes.Text = saleData.Rows[0]["Notes"]?.ToString() ?? "";
                }

                query = @"SELECT fs.Name as ServiceName, si.Quantity, si.UnitPrice, si.TotalPrice
                         FROM SaleItems si
                         INNER JOIN FinancialServices fs ON si.ServiceId = fs.Id
                         WHERE si.SaleId = @SaleId
                         ORDER BY fs.Name";
                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@SaleId", saleId));
                
                if (dataGridView.Columns["ServiceName"] != null)
                    dataGridView.Columns["ServiceName"].HeaderText = "Услуга";
                if (dataGridView.Columns["Quantity"] != null)
                    dataGridView.Columns["Quantity"].HeaderText = "Количество";
                if (dataGridView.Columns["UnitPrice"] != null)
                {
                    dataGridView.Columns["UnitPrice"].HeaderText = "Цена за ед.";
                    dataGridView.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
                }
                if (dataGridView.Columns["TotalPrice"] != null)
                {
                    dataGridView.Columns["TotalPrice"].HeaderText = "Итого";
                    dataGridView.Columns["TotalPrice"].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
