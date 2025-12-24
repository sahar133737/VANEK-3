using System;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class MainMenuForm : BaseForm
    {
        private Label lblWelcome;
        private Label lblRole;
        private Label lblDateTime;
        private Timer timerClock;
        private Panel mainPanel;
        private Panel quickPanel;
        private Panel statsPanel;

        public MainMenuForm()
        {
            InitializeComponent();
            FormTitle = "Главное меню";
            HelpText = HelpTexts.MainMenuForm;
            
            lblWelcome.Text = $"Добро пожаловать, {LoginForm.CurrentUserName}!";
            lblRole.Text = $"Роль: {GetRoleDisplayName(LoginForm.CurrentUserRole)}";
            
            // Запуск часов
            timerClock = new Timer();
            timerClock.Interval = 1000;
            timerClock.Tick += (s, e) => lblDateTime.Text = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
            timerClock.Start();
            lblDateTime.Text = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss");
        }

        private string GetRoleDisplayName(string role)
        {
            switch (role)
            {
                case "Admin": return "Администратор";
                case "Manager": return "Менеджер";
                case "User": return "Пользователь";
                default: return role;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "МКУ \"ЦБУИСХД по Бежицкому району\" - Главное меню";
            this.ClientSize = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 600);
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.WindowState = FormWindowState.Maximized;

            // Боковая панель
            Panel sidePanel = new Panel();
            sidePanel.Dock = DockStyle.Left;
            sidePanel.Width = 280;
            sidePanel.BackColor = Color.FromArgb(45, 45, 48);
            this.Controls.Add(sidePanel);

            // Логотип и название организации
            Label lblLogo = new Label();
            lblLogo.Text = "🏛️";
            lblLogo.Font = new Font("Segoe UI", 48F);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(95, 30);
            lblLogo.AutoSize = true;
            sidePanel.Controls.Add(lblLogo);

            Label lblOrg = new Label();
            lblOrg.Text = "МКУ \"ЦБУИСХД\nпо Бежицкому району\"";
            lblOrg.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOrg.ForeColor = Color.White;
            lblOrg.Location = new Point(20, 120);
            lblOrg.Size = new Size(240, 50);
            lblOrg.TextAlign = ContentAlignment.MiddleCenter;
            sidePanel.Controls.Add(lblOrg);

            // Разделитель
            Panel sep1 = new Panel();
            sep1.BackColor = Color.FromArgb(70, 70, 75);
            sep1.Location = new Point(20, 180);
            sep1.Size = new Size(240, 1);
            sidePanel.Controls.Add(sep1);

            // Кнопки меню
            int btnY = 200;
            int btnHeight = 55;
            int btnWidth = 240;
            int btnSpacing = 10;

            Button btnFinancialServices = CreateMenuButton("📊  Финансовые услуги", btnY);
            btnFinancialServices.Click += btnFinancialServices_Click;
            btnFinancialServices.TabIndex = 0;
            sidePanel.Controls.Add(btnFinancialServices);
            btnY += btnHeight + btnSpacing;

            Button btnSales = CreateMenuButton("💰  Продажи", btnY);
            btnSales.Click += btnSales_Click;
            btnSales.TabIndex = 1;
            sidePanel.Controls.Add(btnSales);
            btnY += btnHeight + btnSpacing;

            Button btnReports = CreateMenuButton("📈  Отчеты", btnY);
            btnReports.Click += btnReports_Click;
            btnReports.TabIndex = 2;
            sidePanel.Controls.Add(btnReports);
            btnY += btnHeight + btnSpacing;

            Button btnUsers = CreateMenuButton("👥  Пользователи", btnY);
            btnUsers.Click += btnUsers_Click;
            btnUsers.TabIndex = 3;
            if (LoginForm.CurrentUserRole != "Admin")
            {
                btnUsers.Enabled = false;
                btnUsers.BackColor = Color.FromArgb(60, 60, 65);
            }
            sidePanel.Controls.Add(btnUsers);
            btnY += btnHeight + btnSpacing;

            Button btnSettings = CreateMenuButton("⚙️  Настройки", btnY);
            btnSettings.Click += btnSettings_Click;
            btnSettings.TabIndex = 4;
            sidePanel.Controls.Add(btnSettings);

            // Кнопка выхода внизу боковой панели
            Button btnExit = new Button();
            btnExit.Text = "🚪  Выход";
            btnExit.Size = new Size(btnWidth, 45);
            btnExit.Location = new Point(20, 0);
            btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnExit.BackColor = Color.FromArgb(183, 28, 28);
            btnExit.ForeColor = Color.White;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Cursor = Cursors.Hand;
            btnExit.Font = new Font("Segoe UI", 11F);
            btnExit.TextAlign = ContentAlignment.MiddleLeft;
            btnExit.Padding = new Padding(20, 0, 0, 0);
            btnExit.TabIndex = 5;
            btnExit.Click += btnExit_Click;
            sidePanel.Controls.Add(btnExit);

            // Основная панель с правильным позиционированием
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(240, 242, 245);
            mainPanel.Padding = new Padding(30, 30, 30, 30);
            this.Controls.Add(mainPanel);

            // Панель заголовка
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 130;
            headerPanel.BackColor = Color.Transparent;
            mainPanel.Controls.Add(headerPanel);

            // Заголовок приветствия
            lblWelcome = new Label();
            lblWelcome.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(45, 45, 48);
            lblWelcome.Location = new Point(0, 10);
            lblWelcome.AutoSize = true;
            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            headerPanel.Controls.Add(lblWelcome);

            lblRole = new Label();
            lblRole.Font = new Font("Segoe UI", 12F);
            lblRole.ForeColor = Color.FromArgb(100, 100, 105);
            lblRole.Location = new Point(2, 50);
            lblRole.AutoSize = true;
            lblRole.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            headerPanel.Controls.Add(lblRole);

            lblDateTime = new Label();
            lblDateTime.Font = new Font("Segoe UI", 11F);
            lblDateTime.ForeColor = Color.FromArgb(100, 100, 105);
            lblDateTime.Location = new Point(2, 75);
            lblDateTime.AutoSize = true;
            lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            headerPanel.Controls.Add(lblDateTime);

            // Панель быстрого доступа с правильным позиционированием
            quickPanel = new Panel();
            quickPanel.Dock = DockStyle.Top;
            quickPanel.Height = 300;
            quickPanel.BackColor = Color.White;
            quickPanel.Padding = new Padding(20);
            quickPanel.Margin = new Padding(0, 20, 0, 20);
            mainPanel.Controls.Add(quickPanel);

            Label lblQuick = new Label();
            lblQuick.Text = "📌  Быстрые действия";
            lblQuick.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblQuick.ForeColor = Color.FromArgb(45, 45, 48);
            lblQuick.Location = new Point(20, 15);
            lblQuick.AutoSize = true;
            quickPanel.Controls.Add(lblQuick);

            // Карточки быстрых действий - используем TableLayoutPanel для правильного позиционирования
            TableLayoutPanel cardsLayout = new TableLayoutPanel();
            cardsLayout.Location = new Point(20, 55);
            cardsLayout.Size = new Size(0, 220);
            cardsLayout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cardsLayout.ColumnCount = 3;
            cardsLayout.RowCount = 1;
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            cardsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cardsLayout.AutoSize = false;
            cardsLayout.Dock = DockStyle.Fill;
            cardsLayout.Margin = new Padding(0);
            quickPanel.Controls.Add(cardsLayout);

            // Карточка 1: Новая продажа
            Action action1 = () => {
                SaleEditForm form = new SaleEditForm();
                if (form.ShowDialog() == DialogResult.OK) { }
            };
            Panel card1 = CreateQuickActionCard("🆕", "Новая продажа", "Создать новую запись о продаже услуг", Color.FromArgb(0, 120, 215));
            card1.Tag = action1;
            card1.Margin = new Padding(5);
            cardsLayout.Controls.Add(card1, 0, 0);

            // Карточка 2: Добавить услугу
            Action action2 = () => {
                FinancialServiceEditForm form = new FinancialServiceEditForm();
                if (form.ShowDialog() == DialogResult.OK) { }
            };
            Panel card2 = CreateQuickActionCard("➕", "Добавить услугу", "Добавить новую финансовую услугу", Color.FromArgb(76, 175, 80));
            card2.Tag = action2;
            card2.Margin = new Padding(5);
            cardsLayout.Controls.Add(card2, 1, 0);

            // Карточка 3: Отчет за сегодня
            Action action3 = () => {
                ReportsForm form = new ReportsForm();
                form.ShowDialog();
            };
            Panel card3 = CreateQuickActionCard("📊", "Отчет за сегодня", "Сформировать дневной отчет", Color.FromArgb(255, 152, 0));
            card3.Tag = action3;
            card3.Margin = new Padding(5);
            cardsLayout.Controls.Add(card3, 2, 0);

            // Панель статистики с правильным позиционированием
            statsPanel = new Panel();
            statsPanel.Dock = DockStyle.Fill;
            statsPanel.BackColor = Color.White;
            statsPanel.Padding = new Padding(20);
            mainPanel.Controls.Add(statsPanel);

            Label lblStats = new Label();
            lblStats.Text = "📈  Статистика";
            lblStats.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(45, 45, 48);
            lblStats.Location = new Point(20, 15);
            lblStats.AutoSize = true;
            statsPanel.Controls.Add(lblStats);

            // Загрузка статистики
            LoadStatistics(statsPanel);

            // Подсказка F1 с правильным позиционированием
            Label lblHint = new Label();
            lblHint.Text = "💡 Нажмите F1 для вызова справки";
            lblHint.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblHint.ForeColor = Color.FromArgb(130, 130, 135);
            lblHint.Location = new Point(30, 0);
            lblHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblHint.AutoSize = true;
            mainPanel.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }

        private Panel CreateQuickActionCard(string icon, string title, string description, Color accentColor)
        {
            Panel card = new Panel();
            card.BackColor = Color.FromArgb(250, 250, 252);
            card.Cursor = Cursors.Hand;
            card.Dock = DockStyle.Fill;

            // Акцентная линия сверху
            Panel accent = new Panel();
            accent.Dock = DockStyle.Top;
            accent.Height = 4;
            accent.BackColor = accentColor;
            card.Controls.Add(accent);

            // Контейнер для содержимого
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(15);
            card.Controls.Add(contentPanel);

            Label lblIcon = new Label();
            lblIcon.Text = icon;
            lblIcon.Font = new Font("Segoe UI", 32F);
            lblIcon.Location = new Point(15, 15);
            lblIcon.AutoSize = true;
            contentPanel.Controls.Add(lblIcon);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(15, 70);
            lblTitle.Size = new Size(200, 25);
            contentPanel.Controls.Add(lblTitle);

            Label lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Segoe UI", 9F);
            lblDesc.ForeColor = Color.FromArgb(100, 100, 105);
            lblDesc.Location = new Point(15, 100);
            lblDesc.Size = new Size(200, 60);
            contentPanel.Controls.Add(lblDesc);

            // Эффект наведения
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(240, 240, 245);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(250, 250, 252);

            // Передача клика на все элементы - используем MouseClick для всех элементов
            MouseEventHandler mouseClickHandler = (s, e) => {
                if (card.Tag != null && card.Tag is Action action)
                {
                    action();
                }
            };
            
            card.MouseClick += mouseClickHandler;
            foreach (Control c in card.Controls)
            {
                c.MouseClick += mouseClickHandler;
                foreach (Control child in c.Controls)
                {
                    child.MouseClick += mouseClickHandler;
                }
            }

            return card;
        }

        private void LoadStatistics(Panel parent)
        {
            try
            {
                // Получаем статистику за сегодня
                var todayStats = DatabaseHelper.ExecuteQuery(
                    @"SELECT 
                        (SELECT COUNT(*) FROM Sales WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)) as TodaySales,
                        (SELECT ISNULL(SUM(TotalAmount), 0) FROM Sales WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)) as TodayRevenue,
                        (SELECT COUNT(*) FROM FinancialServices) as TotalServices,
                        (SELECT COUNT(*) FROM Sales) as TotalSales");

                if (todayStats.Rows.Count > 0)
                {
                    var row = todayStats.Rows[0];
                    
                    // Используем TableLayoutPanel для правильного позиционирования
                    TableLayoutPanel statsLayout = new TableLayoutPanel();
                    statsLayout.Location = new Point(20, 50);
                    statsLayout.Size = new Size(0, 60);
                    statsLayout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    statsLayout.ColumnCount = 4;
                    statsLayout.RowCount = 1;
                    statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    statsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    statsLayout.AutoSize = false;
                    statsLayout.Dock = DockStyle.Fill;
                    statsLayout.Margin = new Padding(0);
                    parent.Controls.Add(statsLayout);

                    CreateStatCard(statsLayout, "🛒", "Продажи сегодня", row["TodaySales"].ToString(), Color.FromArgb(0, 120, 215), 0);
                    CreateStatCard(statsLayout, "💰", "Выручка сегодня", $"{Convert.ToDecimal(row["TodayRevenue"]):N0} ₽", Color.FromArgb(76, 175, 80), 1);
                    CreateStatCard(statsLayout, "📋", "Всего услуг", row["TotalServices"].ToString(), Color.FromArgb(255, 152, 0), 2);
                    CreateStatCard(statsLayout, "📊", "Всего продаж", row["TotalSales"].ToString(), Color.FromArgb(156, 39, 176), 3);
                }
            }
            catch
            {
                Label lblError = new Label();
                lblError.Text = "Не удалось загрузить статистику";
                lblError.Font = new Font("Segoe UI", 9F);
                lblError.ForeColor = Color.Gray;
                lblError.Location = new Point(20, 50);
                lblError.AutoSize = true;
                parent.Controls.Add(lblError);
            }
        }

        private void CreateStatCard(TableLayoutPanel parent, string icon, string title, string value, Color color, int column)
        {
            Panel statCard = new Panel();
            statCard.Dock = DockStyle.Fill;
            statCard.BackColor = Color.Transparent;
            statCard.Margin = new Padding(5);

            Label lblIcon = new Label();
            lblIcon.Text = icon;
            lblIcon.Font = new Font("Segoe UI", 24F);
            lblIcon.Location = new Point(10, 5);
            lblIcon.AutoSize = true;
            statCard.Controls.Add(lblIcon);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblValue.ForeColor = color;
            lblValue.Location = new Point(50, 5);
            lblValue.AutoSize = true;
            statCard.Controls.Add(lblValue);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 9F);
            lblTitle.ForeColor = Color.FromArgb(100, 100, 105);
            lblTitle.Location = new Point(50, 30);
            lblTitle.AutoSize = true;
            statCard.Controls.Add(lblTitle);

            parent.Controls.Add(statCard, column, 0);
        }

        private Button CreateMenuButton(string text, int yPos)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(240, 55);
            btn.Location = new Point(20, yPos);
            btn.BackColor = Color.FromArgb(55, 55, 60);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 120, 215);
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 11F);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            return btn;
        }

        private void btnFinancialServices_Click(object sender, EventArgs e)
        {
            FinancialServicesForm form = new FinancialServicesForm();
            form.ShowDialog();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            SalesForm form = new SalesForm();
            form.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm form = new ReportsForm();
            form.ShowDialog();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (LoginForm.CurrentUserRole == "Admin")
            {
                UsersForm form = new UsersForm();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("У вас нет прав доступа к управлению пользователями!", "Доступ запрещен", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm form = new SettingsForm();
            form.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из программы?", "Выход", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timerClock?.Stop();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerClock?.Stop();
            timerClock?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
