using System;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    /// <summary>
    /// Форма справки, вызываемая по F1
    /// </summary>
    public partial class HelpForm : Form
    {
        public HelpForm(string title, string helpContent)
        {
            InitializeComponent(title, helpContent);
        }

        private void InitializeComponent(string title, string helpContent)
        {
            if (string.IsNullOrEmpty(title)) title = "Справка";
            if (string.IsNullOrEmpty(helpContent)) helpContent = "Справка не доступна.";
            
            this.SuspendLayout();

            // Настройка формы
            this.Text = $"Справка - {title}";
            this.ClientSize = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F1) this.Close(); };

            // Заголовок
            Label lblTitle = new Label();
            lblTitle.Text = $"📖  {title}";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // Разделитель
            Panel separator = new Panel();
            separator.BackColor = Color.FromArgb(0, 120, 215);
            separator.Location = new Point(20, 55);
            separator.Size = new Size(560, 2);
            this.Controls.Add(separator);

            // Текст справки
            RichTextBox rtbHelp = new RichTextBox();
            rtbHelp.Location = new Point(20, 70);
            rtbHelp.Size = new Size(560, 320);
            rtbHelp.ReadOnly = true;
            rtbHelp.BorderStyle = BorderStyle.None;
            rtbHelp.BackColor = Color.FromArgb(250, 250, 252);
            rtbHelp.Font = new Font("Segoe UI", 10F);
            rtbHelp.Text = helpContent;
            this.Controls.Add(rtbHelp);

            // Кнопка закрытия
            Button btnClose = new Button();
            btnClose.Text = "Закрыть";
            btnClose.Size = new Size(100, 35);
            btnClose.Location = new Point(480, 400);
            btnClose.BackColor = Color.FromArgb(0, 120, 215);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Подсказка
            Label lblHint = new Label();
            lblHint.Text = "Нажмите F1 или Escape для закрытия";
            lblHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblHint.ForeColor = Color.Gray;
            lblHint.Location = new Point(20, 408);
            lblHint.AutoSize = true;
            this.Controls.Add(lblHint);

            this.ResumeLayout(false);
        }
    }
}

