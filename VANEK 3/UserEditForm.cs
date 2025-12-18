using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class UserEditForm : Form
    {
        private int? userId;

        public UserEditForm(int? id = null)
        {
            InitializeComponent();
            userId = id;
            cmbRole.Items.AddRange(new[] { "Admin", "User", "Manager" });
            cmbRole.SelectedIndex = 1;
            
            if (id.HasValue)
            {
                LoadUser(id.Value);
                this.Text = "Редактирование пользователя";
            }
            else
            {
                chkIsActive.Checked = true;
                this.Text = "Добавление пользователя";
            }
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
                MessageBox.Show("Введите имя пользователя!", "Ошибка", 
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

