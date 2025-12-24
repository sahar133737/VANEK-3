using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class UsersForm : BaseForm
    {
        private DataGridView dataGridView;
        private ComboBox cmbFilterRole;
        private ComboBox cmbFilterStatus;

        public UsersForm()
        {
            InitializeComponent();
            FormTitle = "Пользователи";
            HelpText = HelpTexts.UsersForm;
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "👥  Пользователи";
            this.ClientSize = new Size(900, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(750, 450);
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.WindowState = FormWindowState.Maximized;

            // Панель фильтров
            Panel pnlFilters = new Panel();
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 80;
            pnlFilters.BackColor = Color.White;
            pnlFilters.Padding = new Padding(15);
            this.Controls.Add(pnlFilters);

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = "👥  Управление пользователями";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(15, 10);
            lblTitle.AutoSize = true;
            pnlFilters.Controls.Add(lblTitle);

            // Фильтр по роли
            Label lblRole = new Label();
            lblRole.Text = "Роль:";
            lblRole.Font = new Font("Segoe UI", 9F);
            lblRole.Location = new Point(15, 45);
            lblRole.AutoSize = true;
            pnlFilters.Controls.Add(lblRole);

            cmbFilterRole = new ComboBox();
            cmbFilterRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterRole.Font = new Font("Segoe UI", 9F);
            cmbFilterRole.Location = new Point(55, 42);
            cmbFilterRole.Size = new Size(150, 25);
            cmbFilterRole.Items.AddRange(new object[] { "Все роли", "Admin", "Manager", "User" });
            cmbFilterRole.SelectedIndex = 0;
            cmbFilterRole.SelectedIndexChanged += (s, e) => LoadUsers();
            pnlFilters.Controls.Add(cmbFilterRole);

            // Фильтр по статусу
            Label lblStatus = new Label();
            lblStatus.Text = "Статус:";
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(230, 45);
            lblStatus.AutoSize = true;
            pnlFilters.Controls.Add(lblStatus);

            cmbFilterStatus = new ComboBox();
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.Font = new Font("Segoe UI", 9F);
            cmbFilterStatus.Location = new Point(285, 42);
            cmbFilterStatus.Size = new Size(150, 25);
            cmbFilterStatus.Items.AddRange(new object[] { "Все статусы", "Активные", "Неактивные" });
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndexChanged += (s, e) => LoadUsers();
            pnlFilters.Controls.Add(cmbFilterStatus);

            // Кнопка сброса
            Button btnClearFilter = new Button();
            btnClearFilter.Text = "✖  Сброс фильтров";
            btnClearFilter.Size = new Size(130, 28);
            btnClearFilter.Location = new Point(460, 40);
            btnClearFilter.BackColor = Color.FromArgb(158, 158, 158);
            btnClearFilter.ForeColor = Color.White;
            btnClearFilter.FlatStyle = FlatStyle.Flat;
            btnClearFilter.FlatAppearance.BorderSize = 0;
            btnClearFilter.Cursor = Cursors.Hand;
            btnClearFilter.Font = new Font("Segoe UI", 9F);
            btnClearFilter.Click += (s, e) => {
                cmbFilterRole.SelectedIndex = 0;
                cmbFilterStatus.SelectedIndex = 0;
            };
            pnlFilters.Controls.Add(btnClearFilter);

            // Таблица данных
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(15, 95);
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
            dataGridView.DoubleClick += (s, e) => btnEdit_Click(s, e);
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

            Button btnRefresh = CreateActionButton("🔄  Обновить", btnX, 12, ButtonColors.Refresh);
            btnRefresh.Click += (s, e) => LoadUsers();
            btnRefresh.TabIndex = 3;
            pnlButtons.Controls.Add(btnRefresh);

            Button btnClose = CreateCloseButton(0, 12, 100, 35);
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.TabIndex = 4;
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

        private void LoadUsers()
        {
            try
            {
                string query = "SELECT Id, Username, FullName, Role, IsActive, CreatedDate FROM Users WHERE 1=1";
                var parameters = new System.Collections.Generic.List<SqlParameter>();

                // Фильтр по роли
                if (cmbFilterRole.SelectedIndex > 0)
                {
                    query += " AND Role = @Role";
                    parameters.Add(new SqlParameter("@Role", cmbFilterRole.SelectedItem.ToString()));
                }

                // Фильтр по статусу
                if (cmbFilterStatus.SelectedIndex == 1)
                {
                    query += " AND IsActive = 1";
                }
                else if (cmbFilterStatus.SelectedIndex == 2)
                {
                    query += " AND IsActive = 0";
                }

                query += " ORDER BY Username";

                dataGridView.DataSource = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                
                if (dataGridView.Columns["Id"] != null)
                    dataGridView.Columns["Id"].Visible = false;
                if (dataGridView.Columns["Username"] != null)
                    dataGridView.Columns["Username"].HeaderText = "Логин";
                if (dataGridView.Columns["FullName"] != null)
                    dataGridView.Columns["FullName"].HeaderText = "Полное имя";
                if (dataGridView.Columns["Role"] != null)
                    dataGridView.Columns["Role"].HeaderText = "Роль";
                if (dataGridView.Columns["IsActive"] != null)
                    dataGridView.Columns["IsActive"].HeaderText = "Активен";
                if (dataGridView.Columns["CreatedDate"] != null)
                {
                    dataGridView.Columns["CreatedDate"].HeaderText = "Дата создания";
                    dataGridView.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserEditForm form = new UserEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            UserEditForm form = new UserEditForm(id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Внимание", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["Id"].Value);
            if (id == LoginForm.CurrentUserId)
            {
                MessageBox.Show("Нельзя удалить текущего пользователя!", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранного пользователя?", "Подтверждение", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Users WHERE Id = @Id";
                    DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id));
                    MessageBox.Show("Пользователь успешно удален!", "Успех", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
