using Microsoft.Data.SqlClient;

namespace Bank_Account_API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string targetDatabase = builder.InitialCatalog;
            
            // 1. Ensure Database exists
            builder.InitialCatalog = "master";
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string checkDbQuery = $"SELECT database_id FROM sys.databases WHERE name = '{targetDatabase}'";
                using (var command = new SqlCommand(checkDbQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        string createDbQuery = $"CREATE DATABASE [{targetDatabase}]";
                        using (var createCommand = new SqlCommand(createDbQuery, connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            // 2. Ensure Tables exist and seed data
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create Banks Table
                string createBanksQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Banks]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [dbo].[Banks] (
                            [BankId] INT IDENTITY(1,1) PRIMARY KEY,
                            [BankName] NVARCHAR(250) NOT NULL,
                            [BranchCode] NVARCHAR(50) NOT NULL,
                            [Address] NVARCHAR(250) NOT NULL,
                            [IFSCCode] NVARCHAR(11) NOT NULL
                        );

                        INSERT INTO [dbo].[Banks] ([BankName], [BranchCode], [Address], [IFSCCode]) VALUES
                        ('State Bank of India', 'SBI001', 'Mumbai, Maharashtra', 'SBIN0000001'),
                        ('HDFC Bank', 'HDFC01', 'Mumbai, Maharashtra', 'HDFC0000001'),
                        ('ICICI Bank', 'ICICI1', 'Mumbai, Maharashtra', 'ICIC0000001'),
                        ('Axis Bank', 'AXIS01', 'Mumbai, Maharashtra', 'UTIB0000001'),
                        ('Punjab National Bank', 'PNB001', 'New Delhi, Delhi', 'PUNB0000001');
                    END";
                
                using (var command = new SqlCommand(createBanksQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create Accounts Table
                string createAccountsQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [dbo].[Accounts] (
                            [AccountId] INT IDENTITY(1,1) PRIMARY KEY,
                            [AccountNumber] NVARCHAR(20) NOT NULL,
                            [AccountHolderName] NVARCHAR(100) NOT NULL,
                            [Balance] DECIMAL(18,2) NOT NULL,
                            [AccountType] NVARCHAR(20) NOT NULL,
                            [BankId] INT NOT NULL,
                            CONSTRAINT [FK_Accounts_Banks_BankId] FOREIGN KEY ([BankId]) REFERENCES [dbo].[Banks] ([BankId]) ON DELETE CASCADE
                        );

                        INSERT INTO [dbo].[Accounts] ([AccountNumber], [AccountHolderName], [Balance], [AccountType], [BankId]) VALUES
                        ('100010001001', 'Thillai Shanmugam', 50000.00, 'Savings', 1),
                        ('200020002002', 'Tamilvani', 75000.00, 'Savings', 2),
                        ('300030003003', 'Dharshini', 60000.00, 'Current', 3),
                        ('400040004004', 'Abinaya', 90000.00, 'Savings', 4);
                    END";

                using (var command = new SqlCommand(createAccountsQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create Transactions Table
                string createTransactionsQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transactions]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [dbo].[Transactions] (
                            [TransactionId] INT IDENTITY(1,1) PRIMARY KEY,
                            [AccountId] INT NOT NULL,
                            [TransactionType] NVARCHAR(10) NOT NULL,
                            [Amount] DECIMAL(18,2) NOT NULL,
                            [TransactionDate] DATETIME NOT NULL,
                            [Description] NVARCHAR(200) NULL,
                            CONSTRAINT [FK_Transactions_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([AccountId]) ON DELETE CASCADE
                        );

                        INSERT INTO [dbo].[Transactions] ([AccountId], [TransactionType], [Amount], [TransactionDate], [Description]) VALUES
                        (1, 'Credit', 10000.00, '2026-08-20T10:00:00', 'Initial Deposit'),
                        (1, 'Debit', 2000.00, '2026-08-22T14:30:00', 'ATM Withdrawal'),
                        (2, 'Credit', 15000.00, '2026-08-21T11:15:00', 'Salary Credit'),
                        (3, 'Debit', 5000.00, '2026-08-23T09:45:00', 'Online Shopping'),
                        (4, 'Credit', 25000.00, '2026-08-24T16:00:00', 'Funds Transfer');
                    END";

                using (var command = new SqlCommand(createTransactionsQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
