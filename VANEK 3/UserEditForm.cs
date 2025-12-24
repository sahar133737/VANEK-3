using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class UserEditForm : BaseForm
    {
        private int? userId;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cmbRole;
        private CheckBox chkIsActive;

        public UserEditForm(int? id = null)
        {
            InitializeComponent();
            FormTitle = id.HasValue ? "Редактирование пользователя" : "Новый пользователь";
            HelpText = HelpTexts.UsersForm;
            
            userId = id;
            
            if (id.HasValue)
            {
                LoadUser(id.Value);
                this.Text = "✏️  Редактирование пользователя";
            }
            else
            {
                chkIsActive.Checked = true;
                cmbRole.SelectedIndex = 2; // User по умолчанию
                this.Text = "➕  Добавление пользователя";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.ClientSize = new Size(450, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Панель с полями
            Panel pnlFields = new Panel();
            pnlFields.Location = new Point(25, 20);
            pnlFields.Size = new Size(400, 300);
            pnlFields.BackColor = Color.White;
            this.Controls.Add(pnlFields);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "👤  Информация о пользователе";
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            pnlFields.Controls.Add(lblTitle);

            // Имя пользователя
            Label lblUsername = new Label();
            lblUsername.Text = "Логин:";
            lblUsername.Font = new Font("Segoe UI", 9F);
            lblUsername.Location = new Point(20, 55);
            lblUsername.AutoSize = true;
            pnlFields.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 9F);
            txtUsername.Location = new Point(130, 52);
            txtUsername.Size = new Size(250, 25);
            pnlFields.Controls.Add(txtUsername);

            // Пароль
            Label lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Font = new Font("Segoe UI", 9F);
            lblPassword.Location = new Point(20, 90);
            lblPassword.AutoSize = true;
            pnlFields.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.Location = new Point(130, 87);
            txtPassword.Size = new Size(250, 25);
            txtPassword.PasswordChar = '●';
            pnlFields.Controls.Add(txtPassword);

            Label lblPasswordHint = new Label();
            lblPasswordHint.Text = "(оставьте пустым для сохранения текущего)";
            lblPasswordHint.Font = new Font("Segoe UI", 7F, FontStyle.Italic);
            lblPasswordHint.ForeColor = Color.Gray;
            lblPasswordHint.Location = new Point(130, 112);
            lblPasswordHint.AutoSize = true;
            pnlFields.Controls.Add(lblPasswordHint);

            // Полное имя
            Label lblFullName = new Label();
            lblFullName.Text = "Полное имя:";
            lblFullName.Font = new Font("Segoe UI", 9F);
            lblFullName.Location = new Point(20, 135);
            lblFullName.AutoSize = true;
            pnlFields.Controls.Add(lblFullName);

            txtFullName = new TextBox();
            txtFullName.Font = new Font("Segoe UI", 9F);
            txtFullName.Location = new Point(130, 132);
            txtFullName.Size = new Size(250, 25);
            pnlFields.Controls.Add(txtFullName);

            // Роль
            Label lblRole = new Label();
            lblRole.Text = "Роль:";
            lblRole.Font = new Font("Segoe UI", 9F);
            lblRole.Location = new Point(20, 170);
            lblRole.AutoSize = true;
            pnlFields.Controls.Add(lblRole);

            cmbRole = new ComboBox();
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Font = new Font("Segoe UI", 9F);
            cmbRole.Location = new Point(130, 167);
            cmbRole.Size = new Size(150, 25);
            cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "User" });
            cmbRole.SelectedIndex = 2;
            pnlFields.Controls.Add(cmbRole);

            // Статус
            chkIsActive = new CheckBox();
            chkIsActive.Text = "  Активный пользователь";
            chkIsActive.Font = new Font("Segoe UI", 9F);
            chkIsActive.Location = new Point(130, 205);
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            pnlFields.Controls.Add(chkIsActive);

            // Кнопки
            Button btnSave = new Button();
            btnSave.Text = "💾 Сохранить";
            btnSave.Size = new Size(120, 35);
            btnSave.Location = new Point(155, 250);
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
            btnCancel.Location = new Point(280, 250);
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
            lblHint.Location = new Point(30, 340);
            lblHint.AutoSize = true;
            this.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }

        private void LoadUser(int id)
        {
            try
            {
                string query = "SELECT Username, FullName, Role, IsActive FROM Users WHERE Id = @Id";
                var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    txtUsername.Text = dt.Rows[0]["Username"].ToString();
                    txtUsername.ReadOnly = true;
                    txtUsername.BackColor = Color.FromArgb(240, 240, 240);
                    txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                    cmbRole.Text = dt.Rows[0]["Role"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
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
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Введите логин пользователя!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите полное имя!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbRole.Text))
            {
                MessageBox.Show("Выберите роль!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (userId.HasValue)
                {
                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        string query = @"UPDATE Users SET FullName = @FullName, Password = @Password, 
                                       Role = @Role, IsActive = @IsActive WHERE Id = @Id";
                        DatabaseHelper.ExecuteNonQuery(query,
                            new SqlParameter("@FullName", txtFullName.Text),
                            new SqlParameter("@Password", txtPassword.Text),
                            new SqlParameter("@Role", cmbRole.Text),
                            new SqlParameter("@IsActive", chkIsActive.Checked),
                            new SqlParameter("@Id", userId.Value));
                    }
                    else
                    {
                        string query = @"UPDATE Users SET FullName = @FullName, Role = @Role, 
                                       IsActive = @IsActive WHERE Id = @Id";
                        DatabaseHelper.ExecuteNonQuery(query,
                            new SqlParameter("@FullName", txtFullName.Text),
                            new SqlParameter("@Role", cmbRole.Text),
                            new SqlParameter("@IsActive", chkIsActive.Checked),
                            new SqlParameter("@Id", userId.Value));
                    }
                    MessageBox.Show("Пользователь успешно обновлен!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Введите пароль для нового пользователя!", "Ошибка", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Focus();
                        return;
                    }

                    string query = @"INSERT INTO Users (Username, Password, FullName, Role, IsActive, CreatedDate) 
                                   VALUES (@Username, @Password, @FullName, @Role, @IsActive, GETDATE())";
                    DatabaseHelper.ExecuteNonQuery(query,
                        new SqlParameter("@Username", txtUsername.Text),
                        new SqlParameter("@Password", txtPassword.Text),
                        new SqlParameter("@FullName", txtFullName.Text),
                        new SqlParameter("@Role", cmbRole.Text),
                        new SqlParameter("@IsActive", chkIsActive.Checked));
                    MessageBox.Show("Пользователь успешно добавлен!", "Успех", 
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
