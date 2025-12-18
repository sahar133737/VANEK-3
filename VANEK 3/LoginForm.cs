using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class LoginForm : Form
    {
        public static int CurrentUserId { get; private set; }
        public static string CurrentUserRole { get; private set; }
        public static string CurrentUserName { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "admin";
            txtPassword.Text = "admin123";
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

