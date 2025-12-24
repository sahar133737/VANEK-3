using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace VANEK_3
{
    public class DatabaseHelper
    {
        private static string connectionString = @"Server=SAHAR\SQLSERVER;Database=FinancialServicesDB;Integrated Security=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 60;
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения запроса: {ex.Message}");
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 60;
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения команды: {ex.Message}");
            }
        }

        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 60;
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка выполнения команды: {ex.Message}");
            }
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    
                    // Создание таблицы Users
                    string createUsersTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                        BEGIN
                            CREATE TABLE Users (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Username NVARCHAR(50) NOT NULL UNIQUE,
                                Password NVARCHAR(255) NOT NULL,
                                FullName NVARCHAR(100) NOT NULL,
                                Role NVARCHAR(50) NOT NULL,
                                IsActive BIT NOT NULL DEFAULT 1,
                                CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                            );
                        END";
                    using (var command = new SqlCommand(createUsersTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Создание таблицы FinancialServices
                    string createFinancialServicesTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialServices')
                        BEGIN
                            CREATE TABLE FinancialServices (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Name NVARCHAR(200) NOT NULL,
                                Category NVARCHAR(100),
                                Price DECIMAL(18,2) NOT NULL,
                                Description NVARCHAR(MAX),
                                CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                            );
                        END";
                    using (var command = new SqlCommand(createFinancialServicesTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Создание таблицы Sales
                    string createSalesTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sales')
                        BEGIN
                            CREATE TABLE Sales (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
                                TotalAmount DECIMAL(18,2) NOT NULL,
                                CustomerName NVARCHAR(200) NOT NULL,
                                Notes NVARCHAR(MAX)
                            );
                        END";
                    using (var command = new SqlCommand(createSalesTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Создание таблицы SaleItems
                    string createSaleItemsTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SaleItems')
                        BEGIN
                            CREATE TABLE SaleItems (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                SaleId INT NOT NULL,
                                ServiceId INT NOT NULL,
                                Quantity INT NOT NULL,
                                UnitPrice DECIMAL(18,2) NOT NULL,
                                TotalPrice DECIMAL(18,2) NOT NULL,
                                FOREIGN KEY (SaleId) REFERENCES Sales(Id) ON DELETE CASCADE,
                                FOREIGN KEY (ServiceId) REFERENCES FinancialServices(Id)
                            );
                        END";
                    using (var command = new SqlCommand(createSaleItemsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Создание администратора по умолчанию
                    string createAdmin = @"
                        IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
                        BEGIN
                            INSERT INTO Users (Username, Password, FullName, Role, IsActive, CreatedDate)
                            VALUES ('admin', 'admin123', 'Администратор', 'Admin', 1, GETDATE());
                        END";
                    using (var command = new SqlCommand(createAdmin, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка инициализации БД: {ex.Message}\n\nУбедитесь, что SQL Server LocalDB установлен.", "Ошибка", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Создает резервную копию базы данных
        /// </summary>
        public static bool BackupDatabase(string backupPath)
        {
            try
            {
                string backupDir = Path.GetDirectoryName(backupPath);
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                using (var connection = GetConnection())
                {
                    connection.Open();
                    string backupQuery = $@"BACKUP DATABASE [FinancialServicesDB] TO DISK = N'{backupPath}' 
                                           WITH NOFORMAT, NOINIT, NAME = N'FinancialServicesDB-Full Database Backup', 
                                           SKIP, NOREWIND, NOUNLOAD, STATS = 10";
                    using (var command = new SqlCommand(backupQuery, connection))
                    {
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания резервной копии: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Восстанавливает базу данных из резервной копии
        /// </summary>
        public static bool RestoreDatabase(string backupPath)
        {
            try
            {
                // Подключаемся к master для восстановления
                string masterConnection = @"Server=SAHAR\SQLSERVER;Database=master;Integrated Security=true;";
                
                using (var connection = new SqlConnection(masterConnection))
                {
                    connection.Open();
                    
                    // Переводим БД в single-user mode
                    string setOfflineQuery = @"IF EXISTS (SELECT name FROM sys.databases WHERE name = N'FinancialServicesDB')
                                               BEGIN
                                                   ALTER DATABASE [FinancialServicesDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                               END";
                    using (var command = new SqlCommand(setOfflineQuery, connection))
                    {
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                    }

                    // Восстанавливаем
                    string restoreQuery = $@"RESTORE DATABASE [FinancialServicesDB] FROM DISK = N'{backupPath}' 
                                            WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10";
                    using (var command = new SqlCommand(restoreQuery, connection))
                    {
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }

                    // Возвращаем в multi-user mode
                    string setOnlineQuery = @"ALTER DATABASE [FinancialServicesDB] SET MULTI_USER";
                    using (var command = new SqlCommand(setOnlineQuery, connection))
                    {
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка восстановления базы данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Заполняет базу данных тестовыми данными
        /// </summary>
        public static void FillSampleData()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    // Добавление пользователей
                    string insertUsers = @"
                        IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'manager')
                        BEGIN
                            INSERT INTO Users (Username, Password, FullName, Role, IsActive)
                            VALUES ('manager', 'manager123', 'Иванов Иван Иванович', 'Manager', 1);
                        END
                        IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'user')
                        BEGIN
                            INSERT INTO Users (Username, Password, FullName, Role, IsActive)
                            VALUES ('user', 'user123', 'Петрова Мария Сергеевна', 'User', 1);
                        END
                        IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'operator')
                        BEGIN
                            INSERT INTO Users (Username, Password, FullName, Role, IsActive)
                            VALUES ('operator', 'operator123', 'Сидоров Алексей Петрович', 'User', 1);
                        END";
                    using (var command = new SqlCommand(insertUsers, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Добавление финансовых услуг
                    string insertServices = @"
                        IF NOT EXISTS (SELECT * FROM FinancialServices WHERE Name = 'Консультация по налогообложению')
                        BEGIN
                            INSERT INTO FinancialServices (Name, Category, Price, Description)
                            VALUES 
                            ('Консультация по налогообложению', 'Консультации', 2500.00, 'Консультационные услуги по вопросам налогообложения юридических и физических лиц'),
                            ('Составление налоговой декларации', 'Налоговые услуги', 3500.00, 'Подготовка и составление налоговой декларации'),
                            ('Бухгалтерское сопровождение (месяц)', 'Бухгалтерия', 15000.00, 'Ежемесячное бухгалтерское обслуживание организации'),
                            ('Аудиторская проверка', 'Аудит', 50000.00, 'Комплексная аудиторская проверка финансовой отчетности'),
                            ('Регистрация ООО', 'Регистрация', 8000.00, 'Полное сопровождение регистрации общества с ограниченной ответственностью'),
                            ('Регистрация ИП', 'Регистрация', 3000.00, 'Регистрация индивидуального предпринимателя'),
                            ('Ликвидация ООО', 'Ликвидация', 25000.00, 'Полное сопровождение процедуры ликвидации юридического лица'),
                            ('Кадровый аутсорсинг', 'Кадры', 12000.00, 'Ведение кадрового учета организации'),
                            ('Расчет заработной платы', 'Бухгалтерия', 5000.00, 'Ежемесячный расчет заработной платы сотрудников'),
                            ('Составление бизнес-плана', 'Консультации', 20000.00, 'Разработка бизнес-плана для инвестиционного проекта'),
                            ('Правовая экспертиза документов', 'Юридические услуги', 4000.00, 'Правовой анализ договоров и документов'),
                            ('Представительство в налоговой', 'Налоговые услуги', 6000.00, 'Представление интересов в налоговых органах'),
                            ('Восстановление бухгалтерского учета', 'Бухгалтерия', 35000.00, 'Восстановление бухгалтерской документации'),
                            ('Оптимизация налогообложения', 'Налоговые услуги', 15000.00, 'Анализ и оптимизация налоговой нагрузки'),
                            ('Сдача отчетности в ФНС', 'Налоговые услуги', 2000.00, 'Подготовка и сдача отчетности в налоговые органы');
                        END";
                    using (var command = new SqlCommand(insertServices, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Добавление продаж
                    string insertSales = @"
                        IF NOT EXISTS (SELECT * FROM Sales WHERE CustomerName = 'ООО ""Ромашка""')
                        BEGIN
                            DECLARE @SaleId1 INT, @SaleId2 INT, @SaleId3 INT, @SaleId4 INT, @SaleId5 INT;
                            DECLARE @ServiceId1 INT, @ServiceId2 INT, @ServiceId3 INT, @ServiceId4 INT, @ServiceId5 INT;
                            
                            SELECT @ServiceId1 = Id FROM FinancialServices WHERE Name = 'Консультация по налогообложению';
                            SELECT @ServiceId2 = Id FROM FinancialServices WHERE Name = 'Составление налоговой декларации';
                            SELECT @ServiceId3 = Id FROM FinancialServices WHERE Name = 'Бухгалтерское сопровождение (месяц)';
                            SELECT @ServiceId4 = Id FROM FinancialServices WHERE Name = 'Регистрация ООО';
                            SELECT @ServiceId5 = Id FROM FinancialServices WHERE Name = 'Аудиторская проверка';
                            
                            -- Продажа 1
                            INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes)
                            VALUES (DATEADD(DAY, -10, GETDATE()), 'ООО ""Ромашка""', 6000.00, 'Постоянный клиент');
                            SET @SaleId1 = SCOPE_IDENTITY();
                            INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice)
                            VALUES (@SaleId1, @ServiceId1, 1, 2500.00, 2500.00),
                                   (@SaleId1, @ServiceId2, 1, 3500.00, 3500.00);
                            
                            -- Продажа 2
                            INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes)
                            VALUES (DATEADD(DAY, -8, GETDATE()), 'ИП Козлов А.В.', 15000.00, NULL);
                            SET @SaleId2 = SCOPE_IDENTITY();
                            INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice)
                            VALUES (@SaleId2, @ServiceId3, 1, 15000.00, 15000.00);
                            
                            -- Продажа 3
                            INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes)
                            VALUES (DATEADD(DAY, -5, GETDATE()), 'ООО ""Технопарк""', 58000.00, 'Крупный клиент');
                            SET @SaleId3 = SCOPE_IDENTITY();
                            INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice)
                            VALUES (@SaleId3, @ServiceId4, 1, 8000.00, 8000.00),
                                   (@SaleId3, @ServiceId5, 1, 50000.00, 50000.00);
                            
                            -- Продажа 4
                            INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes)
                            VALUES (DATEADD(DAY, -3, GETDATE()), 'ООО ""Альфа-Строй""', 30000.00, 'Новый клиент');
                            SET @SaleId4 = SCOPE_IDENTITY();
                            INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice)
                            VALUES (@SaleId4, @ServiceId3, 2, 15000.00, 30000.00);
                            
                            -- Продажа 5
                            INSERT INTO Sales (SaleDate, CustomerName, TotalAmount, Notes)
                            VALUES (DATEADD(DAY, -1, GETDATE()), 'ИП Смирнова Е.Н.', 5500.00, NULL);
                            SET @SaleId5 = SCOPE_IDENTITY();
                            INSERT INTO SaleItems (SaleId, ServiceId, Quantity, UnitPrice, TotalPrice)
                            VALUES (@SaleId5, @ServiceId1, 1, 2500.00, 2500.00),
                                   (@SaleId5, @ServiceId4, 1, 3000.00, 3000.00);
                        END";
                    using (var command = new SqlCommand(insertSales, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Тестовые данные успешно добавлены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заполнения тестовыми данными: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Получает путь к папке с резервными копиями
        /// </summary>
        public static string GetBackupDirectory()
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string backupDir = Path.Combine(appDir, "Backups");
            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }
            return backupDir;
        }

        /// <summary>
        /// Генерирует имя файла резервной копии с текущей датой и временем
        /// </summary>
        public static string GenerateBackupFileName()
        {
            return $"FinancialServicesDB_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
        }
    }
}
