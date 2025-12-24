using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class LoginForm : BaseForm
    {
        public static int CurrentUserId { get; private set; }
        public static string CurrentUserRole { get; private set; }
        public static string CurrentUserName { get; private set; }

        private TextBox txtUsername;
        private TextBox txtPassword;

        public LoginForm()
        {
            InitializeComponent();
            FormTitle = "Авторизация";
            HelpText = HelpTexts.LoginForm;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "Авторизация";
            this.ClientSize = new Size(450, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Логотип
            Label lblLogo = new Label();
            lblLogo.Text = "🏛️";
            lblLogo.Font = new Font("Segoe UI", 48F);
            lblLogo.ForeColor = Color.FromArgb(0, 120, 215);
            lblLogo.Location = new Point(185, 20);
            lblLogo.AutoSize = true;
            this.Controls.Add(lblLogo);

            // Заголовок организации
            Label lblOrg = new Label();
            lblOrg.Text = "МКУ \"ЦБУИСХД по Бежицкому району\"";
            lblOrg.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOrg.ForeColor = Color.FromArgb(45, 45, 48);
            lblOrg.Location = new Point(65, 100);
            lblOrg.AutoSize = true;
            this.Controls.Add(lblOrg);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Система учета финансовых услуг";
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 100, 105);
            lblSubtitle.Location = new Point(125, 125);
            lblSubtitle.AutoSize = true;
            this.Controls.Add(lblSubtitle);

            // Панель входа
            Panel loginPanel = new Panel();
            loginPanel.Location = new Point(50, 160);
            loginPanel.Size = new Size(350, 180);
            loginPanel.BackColor = Color.White;
            this.Controls.Add(loginPanel);

            // Имя пользователя
            Label lblUsername = new Label();
            lblUsername.Text = "👤  Имя пользователя";
            lblUsername.Font = new Font("Segoe UI", 9F);
            lblUsername.ForeColor = Color.FromArgb(100, 100, 105);
            lblUsername.Location = new Point(20, 20);
            lblUsername.AutoSize = true;
            loginPanel.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(20, 42);
            txtUsername.Size = new Size(310, 27);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Text = "admin";
            loginPanel.Controls.Add(txtUsername);

            // Пароль
            Label lblPassword = new Label();
            lblPassword.Text = "🔒  Пароль";
            lblPassword.Font = new Font("Segoe UI", 9F);
            lblPassword.ForeColor = Color.FromArgb(100, 100, 105);
            lblPassword.Location = new Point(20, 80);
            lblPassword.AutoSize = true;
            loginPanel.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(20, 102);
            txtPassword.Size = new Size(310, 27);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '●';
            txtPassword.Text = "admin123";
            txtPassword.KeyPress += txtPassword_KeyPress;
            loginPanel.Controls.Add(txtPassword);

            // Кнопка входа
            Button btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Size = new Size(150, 40);
            btnLogin.Location = new Point(20, 140);
            btnLogin.BackColor = Color.FromArgb(0, 120, 215);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.Click += btnLogin_Click;
            btnLogin.TabIndex = 0;
            loginPanel.Controls.Add(btnLogin);

            // Кнопка отмены
            Button btnCancel = new Button();
            btnCancel.Text = "Выход";
            btnCancel.Size = new Size(150, 40);
            btnCancel.Location = new Point(180, 140);
            btnCancel.BackColor = Color.FromArgb(97, 97, 97);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Click += btnCancel_Click;
            btnCancel.TabIndex = 1;
            loginPanel.Controls.Add(btnCancel);

            // Подсказка F1
            Label lblHint = new Label();
            lblHint.Text = "💡 Нажмите F1 для справки";
            lblHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblHint.ForeColor = Color.FromArgb(150, 150, 155);
            lblHint.Location = new Point(155, 350);
            lblHint.AutoSize = true;
            this.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "SELECT Id, Username, FullName, Role FROM Users WHERE Username = @Username AND Password = @Password AND IsActive = 1";
                DataTable dt = DatabaseHelper.ExecuteQuery(query,
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password));

                if (dt.Rows.Count > 0)
                {
                    CurrentUserId = Convert.ToInt32(dt.Rows[0]["Id"]);
                    CurrentUserName = dt.Rows[0]["FullName"].ToString();
                    CurrentUserRole = dt.Rows[0]["Role"].ToString();

                    this.Hide();
                    MainMenuForm mainForm = new MainMenuForm();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль!", "Ошибка входа", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
