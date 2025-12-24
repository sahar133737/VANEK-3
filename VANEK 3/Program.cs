using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VANEK_3
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Инициализация базы данных
                try
                {
                    DatabaseHelper.InitializeDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}\n\n" +
                        "Убедитесь, что:\n" +
                        "1. SQL Server запущен\n" +
                        "2. База данных доступна\n" +
                        "3. У вас есть права доступа\n\n" +
                        "Приложение будет закрыто.", 
                        "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при запуске приложения:\n\n{ex.Message}\n\n" +
                    $"Тип ошибки: {ex.GetType().Name}\n" +
                    $"Стек вызовов:\n{ex.StackTrace}", 
                    "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
