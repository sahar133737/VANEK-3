using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
    }
}

