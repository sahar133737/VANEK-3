using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class ReportsForm : BaseForm
    {
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private ComboBox cmbReportType;
        private DataGridView dataGridView;
        private Label lblSummary;
        private string currentReportType = "";
        private PrintDocument printDocument;
        private DataTable printData;

        public ReportsForm()
        {
            InitializeComponent();
            FormTitle = "Отчеты";
            HelpText = HelpTexts.ReportsForm;
            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;
            cmbReportType.SelectedIndex = 0;
            
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "📈  Отчеты";
            this.ClientSize = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(900, 550);
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
            lblTitle.Text = "📊  Формирование отчетов";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(15, 10);
            lblTitle.AutoSize = true;
            pnlFilters.Controls.Add(lblTitle);

            // Тип отчета
            Label lblReportType = new Label();
            lblReportType.Text = "Тип отчета:";
            lblReportType.Font = new Font("Segoe UI", 9F);
            lblReportType.Location = new Point(15, 50);
            lblReportType.AutoSize = true;
            pnlFilters.Controls.Add(lblReportType);

            cmbReportType = new ComboBox();
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Font = new Font("Segoe UI", 9F);
            cmbReportType.Location = new Point(95, 47);
            cmbReportType.Size = new Size(200, 25);
            cmbReportType.Items.AddRange(new object[] {
                "📊 Отчет по продажам",
                "📈 Отчет по услугам",
                "📅 Дневной отчет"
            });
            pnlFilters.Controls.Add(cmbReportType);

            // Дата с
            Label lblFrom = new Label();
            lblFrom.Text = "С:";
            lblFrom.Font = new Font("Segoe UI", 9F);
            lblFrom.Location = new Point(320, 50);
            lblFrom.AutoSize = true;
            pnlFilters.Controls.Add(lblFrom);

            dtpFrom = new DateTimePicker();
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Font = new Font("Segoe UI", 9F);
            dtpFrom.Location = new Point(340, 47);
            dtpFrom.Size = new Size(120, 25);
            pnlFilters.Controls.Add(dtpFrom);

            // Дата по
            Label lblTo = new Label();
            lblTo.Text = "По:";
            lblTo.Font = new Font("Segoe UI", 9F);
            lblTo.Location = new Point(480, 50);
            lblTo.AutoSize = true;
            pnlFilters.Controls.Add(lblTo);

            dtpTo = new DateTimePicker();
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Font = new Font("Segoe UI", 9F);
            dtpTo.Location = new Point(510, 47);
            dtpTo.Size = new Size(120, 25);
            pnlFilters.Controls.Add(dtpTo);

            // Кнопка формирования
            Button btnGenerate = CreateActionButton("📋  Сформировать", 660, 45, ButtonColors.Edit, 150, 32);
            btnGenerate.Click += BtnGenerate_Click;
            pnlFilters.Controls.Add(btnGenerate);

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
            this.Controls.Add(dataGridView);

            // Панель итогов и кнопок
            Panel pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 60;
            pnlBottom.BackColor = Color.White;
            this.Controls.Add(pnlBottom);

            // Итого
            lblSummary = new Label();
            lblSummary.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(45, 45, 48);
            lblSummary.Location = new Point(15, 18);
            lblSummary.AutoSize = true;
            pnlBottom.Controls.Add(lblSummary);

            // Кнопки экспорта и печати
            int btnX = 450;
            
            Button btnExportExcel = CreateActionButton("📥  Excel", btnX, 12, ButtonColors.Export, 120, 35);
            btnExportExcel.Click += BtnExportExcel_Click;
            pnlBottom.Controls.Add(btnExportExcel);
            btnX += 130;

            Button btnExportCSV = CreateActionButton("📄  CSV", btnX, 12, ButtonColors.Export, 110, 35);
            btnExportCSV.Click += BtnExportCSV_Click;
            pnlBottom.Controls.Add(btnExportCSV);
            btnX += 120;

            Button btnPrint = CreateActionButton("🖨️  Печать", btnX, 12, ButtonColors.Print, 120, 35);
            btnPrint.Click += BtnPrint_Click;
            pnlBottom.Controls.Add(btnPrint);
            btnX += 130;

            Button btnClose = CreateCloseButton(btnX, 12, 100, 35);
            btnClose.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnClose);

            // Корректируем размер DataGridView
            this.Resize += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 30, 
                    this.ClientSize.Height - pnlFilters.Height - pnlBottom.Height - 25);
            };

            this.Load += (s, e) => {
                dataGridView.Size = new Size(this.ClientSize.Width - 30, 
                    this.ClientSize.Height - pnlFilters.Height - pnlBottom.Height - 25);
            };

            this.ResumeLayout(false);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            switch (cmbReportType.SelectedIndex)
            {
                case 0:
                    GenerateSalesReport();
                    currentReportType = "Отчет_по_продажам";
                    break;
                case 1:
                    GenerateServicesReport();
                    currentReportType = "Отчет_по_услугам";
                    break;
                case 2:
                    GenerateDailyReport();
                    currentReportType = "Дневной_отчет";
                    break;
            }
        }

        private void GenerateSalesReport()
        {
            try
            {
                string query = @"SELECT s.Id, s.SaleDate, s.CustomerName, s.TotalAmount,
                                (SELECT COUNT(*) FROM SaleItems WHERE SaleId = s.Id) as ItemsCount
                                FROM Sales s
                                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                                ORDER BY s.SaleDate DESC";
                var dt = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                
                dataGridView.DataSource = dt;
                printData = dt.Copy();
                
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
                if (dataGridView.Columns["ItemsCount"] != null)
                    dataGridView.Columns["ItemsCount"].HeaderText = "Позиций";

                decimal total = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["TotalAmount"].Value != null)
                        total += Convert.ToDecimal(row.Cells["TotalAmount"].Value);
                }
                lblSummary.Text = $"📊 Всего продаж: {dataGridView.Rows.Count}  |  💰 Общая сумма: {total:N2} руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateServicesReport()
        {
            try
            {
                string query = @"SELECT fs.Name, fs.Category, fs.Price,
                                ISNULL(COUNT(si.Id), 0) as SalesCount,
                                ISNULL(SUM(si.Quantity), 0) as TotalQuantity,
                                ISNULL(SUM(si.TotalPrice), 0) as TotalRevenue
                                FROM FinancialServices fs
                                LEFT JOIN SaleItems si ON fs.Id = si.ServiceId
                                LEFT JOIN Sales s ON si.SaleId = s.Id AND s.SaleDate BETWEEN @FromDate AND @ToDate
                                GROUP BY fs.Id, fs.Name, fs.Category, fs.Price
                                ORDER BY TotalRevenue DESC";
                var dt = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                
                dataGridView.DataSource = dt;
                printData = dt.Copy();
                
                if (dataGridView.Columns["Name"] != null)
                    dataGridView.Columns["Name"].HeaderText = "Наименование";
                if (dataGridView.Columns["Category"] != null)
                    dataGridView.Columns["Category"].HeaderText = "Категория";
                if (dataGridView.Columns["Price"] != null)
                {
                    dataGridView.Columns["Price"].HeaderText = "Цена";
                    dataGridView.Columns["Price"].DefaultCellStyle.Format = "N2";
                }
                if (dataGridView.Columns["SalesCount"] != null)
                    dataGridView.Columns["SalesCount"].HeaderText = "Кол-во продаж";
                if (dataGridView.Columns["TotalQuantity"] != null)
                    dataGridView.Columns["TotalQuantity"].HeaderText = "Общее кол-во";
                if (dataGridView.Columns["TotalRevenue"] != null)
                {
                    dataGridView.Columns["TotalRevenue"].HeaderText = "Выручка";
                    dataGridView.Columns["TotalRevenue"].DefaultCellStyle.Format = "N2";
                }

                decimal totalRevenue = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["TotalRevenue"].Value != null && row.Cells["TotalRevenue"].Value != DBNull.Value)
                        totalRevenue += Convert.ToDecimal(row.Cells["TotalRevenue"].Value);
                }
                lblSummary.Text = $"📈 Всего услуг: {dataGridView.Rows.Count}  |  💰 Общая выручка: {totalRevenue:N2} руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateDailyReport()
        {
            try
            {
                string query = @"SELECT CAST(s.SaleDate AS DATE) as SaleDate,
                                COUNT(s.Id) as SalesCount,
                                SUM(s.TotalAmount) as DailyTotal
                                FROM Sales s
                                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                                GROUP BY CAST(s.SaleDate AS DATE)
                                ORDER BY SaleDate DESC";
                var dt = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@FromDate", dtpFrom.Value.Date),
                    new SqlParameter("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                
                dataGridView.DataSource = dt;
                printData = dt.Copy();
                
                if (dataGridView.Columns["SaleDate"] != null)
                {
                    dataGridView.Columns["SaleDate"].HeaderText = "Дата";
                    dataGridView.Columns["SaleDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
                if (dataGridView.Columns["SalesCount"] != null)
                    dataGridView.Columns["SalesCount"].HeaderText = "Количество продаж";
                if (dataGridView.Columns["DailyTotal"] != null)
                {
                    dataGridView.Columns["DailyTotal"].HeaderText = "Сумма за день";
                    dataGridView.Columns["DailyTotal"].DefaultCellStyle.Format = "N2";
                }

                decimal total = 0;
                int count = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["DailyTotal"].Value != null)
                    {
                        total += Convert.ToDecimal(row.Cells["DailyTotal"].Value);
                        count += Convert.ToInt32(row.Cells["SalesCount"].Value);
                    }
                }
                lblSummary.Text = $"📅 Дней: {dataGridView.Rows.Count}  |  🛒 Продаж: {count}  |  💰 Сумма: {total:N2} руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования отчета: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для печати. Сначала сформируйте отчет.", 
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (PrintDialog printDialog = new PrintDialog())
                {
                    printDialog.Document = printDocument;
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка печати: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (printData == null || printData.Rows.Count == 0)
                return;

            Graphics g = e.Graphics;
            Font titleFont = new Font("Segoe UI", 16F, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            Font dataFont = new Font("Segoe UI", 9F);
            Font summaryFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            float yPos = 50;
            float leftMargin = 50;
            float rightMargin = e.MarginBounds.Right;
            float lineHeight = 25;
            float headerHeight = 30;

            // Заголовок
            string title = cmbReportType.SelectedItem?.ToString() ?? "Отчет";
            title = title.Replace("📊", "").Replace("📈", "").Replace("📅", "").Trim();
            g.DrawString(title, titleFont, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            // Период
            g.DrawString($"Период: {dtpFrom.Value:dd.MM.yyyy} - {dtpTo.Value:dd.MM.yyyy}", 
                dataFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            // Дата формирования
            g.DrawString($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}", 
                dataFont, Brushes.Gray, leftMargin, yPos);
            yPos += 30;

            // Заголовки таблицы
            float[] columnWidths = new float[printData.Columns.Count];
            float totalWidth = rightMargin - leftMargin;
            float columnWidth = totalWidth / printData.Columns.Count;

            for (int i = 0; i < printData.Columns.Count; i++)
            {
                columnWidths[i] = columnWidth;
                string headerText = printData.Columns[i].ColumnName;
                
                // Получаем заголовок из DataGridView если возможно
                if (dataGridView.Columns[headerText] != null)
                {
                    headerText = dataGridView.Columns[headerText].HeaderText;
                }

                RectangleF rect = new RectangleF(leftMargin + i * columnWidth, yPos, columnWidth, headerHeight);
                g.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 48)), rect);
                g.DrawString(headerText, headerFont, Brushes.White, rect, 
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            yPos += headerHeight;

            // Данные
            int rowsPrinted = 0;
            int maxRowsPerPage = (int)((e.MarginBounds.Bottom - yPos) / lineHeight);

            for (int i = 0; i < printData.Rows.Count && rowsPrinted < maxRowsPerPage; i++)
            {
                DataRow row = printData.Rows[i];
                for (int j = 0; j < printData.Columns.Count; j++)
                {
                    string cellValue = row[j]?.ToString() ?? "";
                    
                    // Форматирование дат и чисел
                    if (row[j] is DateTime dt)
                        cellValue = dt.ToString("dd.MM.yyyy HH:mm");
                    else if (row[j] is decimal dec)
                        cellValue = dec.ToString("N2");

                    RectangleF rect = new RectangleF(leftMargin + j * columnWidth, yPos, columnWidth, lineHeight);
                    g.DrawRectangle(Pens.LightGray, rect.X, rect.Y, rect.Width, rect.Height);
                    g.DrawString(cellValue, dataFont, Brushes.Black, rect, 
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                yPos += lineHeight;
                rowsPrinted++;
            }

            // Итоговая строка
            if (rowsPrinted < printData.Rows.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                yPos += 10;
                g.DrawString(lblSummary.Text, summaryFont, Brushes.Black, leftMargin, yPos);
            }
        }

        private void ExportToExcel()
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта. Сначала сформируйте отчет.", 
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel файлы (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                    sfd.FileName = $"{currentReportType}_{DateTime.Now:yyyyMMdd_HHmmss}";
                    sfd.Title = "Сохранить отчет";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        
                        // Экспорт через XML SpreadsheetML (работает без Excel)
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
                        sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                        sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                        sb.AppendLine("<Styles>");
                        sb.AppendLine("<Style ss:ID=\"Header\"><Font ss:Bold=\"1\"/><Interior ss:Color=\"#4472C4\" ss:Pattern=\"Solid\"/><Font ss:Color=\"#FFFFFF\"/></Style>");
                        sb.AppendLine("<Style ss:ID=\"Data\"><Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/></Borders></Style>");
                        sb.AppendLine("</Styles>");
                        sb.AppendLine("<Worksheet ss:Name=\"Отчет\">");
                        sb.AppendLine("<Table>");

                        // Заголовки
                        sb.AppendLine("<Row>");
                        foreach (DataGridViewColumn col in dataGridView.Columns)
                        {
                            if (col.Visible)
                            {
                                sb.AppendLine($"<Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">{EscapeXml(col.HeaderText)}</Data></Cell>");
                            }
                        }
                        sb.AppendLine("</Row>");

                        // Данные
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            sb.AppendLine("<Row>");
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (dataGridView.Columns[cell.ColumnIndex].Visible)
                                {
                                    string value = cell.Value?.ToString() ?? "";
                                    string type = "String";
                                    
                                    if (cell.Value is decimal || cell.Value is double || cell.Value is int)
                                    {
                                        type = "Number";
                                        value = Convert.ToString(cell.Value, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else if (cell.Value is DateTime dt)
                                    {
                                        value = dt.ToString("dd.MM.yyyy HH:mm");
                                    }
                                    
                                    sb.AppendLine($"<Cell ss:StyleID=\"Data\"><Data ss:Type=\"{type}\">{EscapeXml(value)}</Data></Cell>");
                                }
                            }
                            sb.AppendLine("</Row>");
                        }

                        sb.AppendLine("</Table>");
                        sb.AppendLine("</Worksheet>");
                        sb.AppendLine("</Workbook>");

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        
                        Cursor = Cursors.Default;
                        
                        if (MessageBox.Show($"Отчет успешно сохранен!\n\n{sfd.FileName}\n\nОткрыть файл?", 
                            "Экспорт завершен", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV()
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта. Сначала сформируйте отчет.", 
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV файлы (*.csv)|*.csv";
                    sfd.FileName = $"{currentReportType}_{DateTime.Now:yyyyMMdd_HHmmss}";
                    sfd.Title = "Сохранить отчет";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        
                        StringBuilder sb = new StringBuilder();

                        // Заголовки
                        var headers = new System.Collections.Generic.List<string>();
                        foreach (DataGridViewColumn col in dataGridView.Columns)
                        {
                            if (col.Visible)
                            {
                                headers.Add($"\"{col.HeaderText}\"");
                            }
                        }
                        sb.AppendLine(string.Join(";", headers));

                        // Данные
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            var values = new System.Collections.Generic.List<string>();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (dataGridView.Columns[cell.ColumnIndex].Visible)
                                {
                                    string value = cell.Value?.ToString() ?? "";
                                    value = value.Replace("\"", "\"\"");
                                    values.Add($"\"{value}\"");
                                }
                            }
                            sb.AppendLine(string.Join(";", values));
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        
                        Cursor = Cursors.Default;
                        
                        MessageBox.Show($"Отчет успешно сохранен!\n\n{sfd.FileName}", 
                            "Экспорт завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&apos;");
        }
    }
}
