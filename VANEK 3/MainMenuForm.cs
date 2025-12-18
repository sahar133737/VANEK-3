using System;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            lblWelcome.Text = $"Добро пожаловать, {LoginForm.CurrentUserName}!";
            lblRole.Text = $"Роль: {LoginForm.CurrentUserRole}";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}

