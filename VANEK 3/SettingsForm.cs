using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VANEK_3
{
    public partial class SettingsForm : BaseForm
    {
        public SettingsForm()
        {
            InitializeComponent();
            FormTitle = "Настройки";
            HelpText = HelpTexts.SettingsForm;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройка формы
            this.Text = "Настройки и обслуживание";
            this.ClientSize = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Группа резервного копирования
            GroupBox grpBackup = new GroupBox();
            grpBackup.Text = "📦  Резервное копирование";
            grpBackup.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpBackup.Location = new Point(20, 20);
            grpBackup.Size = new Size(560, 180);
            grpBackup.BackColor = Color.White;
            this.Controls.Add(grpBackup);

            Label lblBackupInfo = new Label();
            lblBackupInfo.Text = "Создание и восстановление резервных копий базы данных.\nРезервные копии сохраняются в папке 'Backups' рядом с программой.";
            lblBackupInfo.Font = new Font("Segoe UI", 9F);
            lblBackupInfo.Location = new Point(15, 30);
            lblBackupInfo.Size = new Size(530, 40);
            grpBackup.Controls.Add(lblBackupInfo);

            Button btnCreateBackup = new Button();
            btnCreateBackup.Text = "💾  Создать резервную копию";
            btnCreateBackup.Size = new Size(250, 40);
            btnCreateBackup.Location = new Point(15, 80);
            btnCreateBackup.BackColor = Color.FromArgb(0, 120, 215);
            btnCreateBackup.ForeColor = Color.White;
            btnCreateBackup.FlatStyle = FlatStyle.Flat;
            btnCreateBackup.FlatAppearance.BorderSize = 0;
            btnCreateBackup.Cursor = Cursors.Hand;
            btnCreateBackup.Font = new Font("Segoe UI", 9F);
            btnCreateBackup.Click += BtnCreateBackup_Click;
            grpBackup.Controls.Add(btnCreateBackup);

            Button btnRestoreBackup = new Button();
            btnRestoreBackup.Text = "📂  Восстановить из копии";
            btnRestoreBackup.Size = new Size(250, 40);
            btnRestoreBackup.Location = new Point(290, 80);
            btnRestoreBackup.BackColor = Color.FromArgb(76, 175, 80);
            btnRestoreBackup.ForeColor = Color.White;
            btnRestoreBackup.FlatStyle = FlatStyle.Flat;
            btnRestoreBackup.FlatAppearance.BorderSize = 0;
            btnRestoreBackup.Cursor = Cursors.Hand;
            btnRestoreBackup.Font = new Font("Segoe UI", 9F);
            btnRestoreBackup.Click += BtnRestoreBackup_Click;
            grpBackup.Controls.Add(btnRestoreBackup);

            Button btnOpenBackupFolder = new Button();
            btnOpenBackupFolder.Text = "📁  Открыть папку с копиями";
            btnOpenBackupFolder.Size = new Size(250, 35);
            btnOpenBackupFolder.Location = new Point(15, 130);
            btnOpenBackupFolder.BackColor = Color.FromArgb(158, 158, 158);
            btnOpenBackupFolder.ForeColor = Color.White;
            btnOpenBackupFolder.FlatStyle = FlatStyle.Flat;
            btnOpenBackupFolder.FlatAppearance.BorderSize = 0;
            btnOpenBackupFolder.Cursor = Cursors.Hand;
            btnOpenBackupFolder.Font = new Font("Segoe UI", 9F);
            btnOpenBackupFolder.Click += BtnOpenBackupFolder_Click;
            grpBackup.Controls.Add(btnOpenBackupFolder);

            // Группа тестовых данных
            GroupBox grpTestData = new GroupBox();
            grpTestData.Text = "📊  Тестовые данные";
            grpTestData.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpTestData.Location = new Point(20, 220);
            grpTestData.Size = new Size(560, 120);
            grpTestData.BackColor = Color.White;
            this.Controls.Add(grpTestData);

            Label lblTestDataInfo = new Label();
            lblTestDataInfo.Text = "Заполнение базы данных демонстрационными данными.\nВнимание: существующие данные будут сохранены.";
            lblTestDataInfo.Font = new Font("Segoe UI", 9F);
            lblTestDataInfo.Location = new Point(15, 30);
            lblTestDataInfo.Size = new Size(530, 35);
            grpTestData.Controls.Add(lblTestDataInfo);

            Button btnFillTestData = new Button();
            btnFillTestData.Text = "📝  Заполнить тестовыми данными";
            btnFillTestData.Size = new Size(250, 40);
            btnFillTestData.Location = new Point(15, 70);
            btnFillTestData.BackColor = Color.FromArgb(255, 152, 0);
            btnFillTestData.ForeColor = Color.White;
            btnFillTestData.FlatStyle = FlatStyle.Flat;
            btnFillTestData.FlatAppearance.BorderSize = 0;
            btnFillTestData.Cursor = Cursors.Hand;
            btnFillTestData.Font = new Font("Segoe UI", 9F);
            btnFillTestData.Click += BtnFillTestData_Click;
            grpTestData.Controls.Add(btnFillTestData);

            // Группа информации
            GroupBox grpInfo = new GroupBox();
            grpInfo.Text = "ℹ️  Информация о системе";
            grpInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpInfo.Location = new Point(20, 360);
            grpInfo.Size = new Size(560, 80);
            grpInfo.BackColor = Color.White;
            this.Controls.Add(grpInfo);

            Label lblVersion = new Label();
            lblVersion.Text = "Версия: 1.0.0\nМКУ \"ЦБУИСХД по Бежицкому району\"\nРазработано в 2024 году";
            lblVersion.Font = new Font("Segoe UI", 9F);
            lblVersion.Location = new Point(15, 25);
            lblVersion.Size = new Size(530, 50);
            grpInfo.Controls.Add(lblVersion);

            // Кнопка закрытия
            Button btnClose = new Button();
            btnClose.Text = "Закрыть";
            btnClose.Size = new Size(100, 35);
            btnClose.Location = new Point(480, 455);
            btnClose.BackColor = Color.FromArgb(97, 97, 97);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Подсказка F1
            Label lblHint = new Label();
            lblHint.Text = "Нажмите F1 для справки";
            lblHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblHint.ForeColor = Color.Gray;
            lblHint.Location = new Point(20, 463);
            lblHint.AutoSize = true;
            this.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }

        private void BtnCreateBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string backupDir = DatabaseHelper.GetBackupDirectory();
                string backupFileName = DatabaseHelper.GenerateBackupFileName();
                string backupPath = Path.Combine(backupDir, backupFileName);

                if (MessageBox.Show($"Создать резервную копию базы данных?\n\nФайл: {backupFileName}", 
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    
                    if (DatabaseHelper.BackupDatabase(backupPath))
                    {
                        MessageBox.Show($"Резервная копия успешно создана!\n\nФайл: {backupPath}", 
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestoreBackup_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Выберите файл резервной копии";
                    ofd.Filter = "Файлы резервных копий (*.bak)|*.bak|Все файлы (*.*)|*.*";
                    ofd.InitialDirectory = DatabaseHelper.GetBackupDirectory();

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (MessageBox.Show(
                            "ВНИМАНИЕ! Восстановление базы данных заменит все текущие данные!\n\n" +
                            "Вы уверены, что хотите продолжить?", 
                            "Подтверждение восстановления", 
                            MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Cursor = Cursors.WaitCursor;
                            
                            if (DatabaseHelper.RestoreDatabase(ofd.FileName))
                            {
                                MessageBox.Show("База данных успешно восстановлена!\n\nПрограмма будет перезапущена.", 
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Restart();
                            }
                            
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOpenBackupFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string backupDir = DatabaseHelper.GetBackupDirectory();
                System.Diagnostics.Process.Start("explorer.exe", backupDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия папки: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFillTestData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Добавить тестовые данные в базу?\n\n" +
                "Будут добавлены:\n" +
                "• Пользователи (3 шт.)\n" +
                "• Финансовые услуги (15 шт.)\n" +
                "• Примеры продаж (5 шт.)", 
                "Подтверждение", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                DatabaseHelper.FillSampleData();
                Cursor = Cursors.Default;
            }
        }
    }
}

