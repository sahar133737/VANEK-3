using System;
using System.Drawing;
using System.Windows.Forms;

namespace VANEK_3
{
    /// <summary>
    /// Базовый класс формы с общей функциональностью:
    /// - Справка по F1
    /// - Поддержка масштабирования
    /// - Навигация по Tab
    /// </summary>
    public class BaseForm : Form
    {
        protected string HelpText { get; set; } = "Справка не доступна для этой формы.";
        protected string FormTitle { get; set; } = "Форма";

        public BaseForm()
        {
            this.KeyPreview = true;
            this.KeyDown += BaseForm_KeyDown;
            this.Load += BaseForm_Load;
            this.Icon = SystemIcons.Application;
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация при загрузке формы
        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                ShowHelp();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }

        protected virtual void ShowHelp()
        {
            HelpForm helpForm = new HelpForm(FormTitle, HelpText);
            helpForm.ShowDialog(this);
        }

        /// <summary>
        /// Настраивает форму для полноэкранного режима с возможностью масштабирования
        /// </summary>
        protected void ConfigureForFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(800, 600);
        }

        /// <summary>
        /// Настраивает DataGridView для автоматического масштабирования
        /// </summary>
        protected void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(224, 224, 224);
        }

        /// <summary>
        /// Создает унифицированную кнопку действия
        /// </summary>
        protected Button CreateActionButton(string text, int x, int y, Color backColor, int width = 0, int height = 35)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(width > 0 ? width : (text.Length > 15 ? 140 : 115), height);
            btn.Location = new Point(x, y);
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(255, backColor.R + 20),
                Math.Min(255, backColor.G + 20),
                Math.Min(255, backColor.B + 20));
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9F);
            return btn;
        }

        /// <summary>
        /// Создает стандартную кнопку закрытия
        /// </summary>
        protected Button CreateCloseButton(int x, int y, int width = 100, int height = 35)
        {
            return CreateActionButton("Закрыть", x, y, Color.FromArgb(97, 97, 97), width, height);
        }

        /// <summary>
        /// Стандартные цвета для кнопок
        /// </summary>
        protected static class ButtonColors
        {
            public static Color Add = Color.FromArgb(76, 175, 80);
            public static Color Edit = Color.FromArgb(0, 120, 215);
            public static Color Delete = Color.FromArgb(244, 67, 54);
            public static Color Refresh = Color.FromArgb(255, 152, 0);
            public static Color View = Color.FromArgb(156, 39, 176);
            public static Color Export = Color.FromArgb(76, 175, 80);
            public static Color Print = Color.FromArgb(33, 150, 243);
            public static Color Close = Color.FromArgb(97, 97, 97);
            public static Color Save = Color.FromArgb(0, 120, 215);
            public static Color Cancel = Color.FromArgb(97, 97, 97);
        }
    }
}
