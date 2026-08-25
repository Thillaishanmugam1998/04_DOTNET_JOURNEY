using Bank_Account_API.Models;
using Microsoft.Data.SqlClient;

namespace Bank_Account_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection is not configured.");
        }

        public async Task<IEnumerable<Accounts>> GetAccountsAsync()
        {
            var accounts = new List<Accounts>();
            const string query = "SELECT AccountId, AccountNumber, AccountHolderName, Balance, AccountType, BankId FROM Accounts";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            accounts.Add(new Accounts
                            {
                                AccountId = reader.GetInt32(0),
                                AccountNumber = reader.GetString(1),
                                AccountHolderName = reader.GetString(2),
                                Balance = reader.GetDecimal(3),
                                AccountType = reader.GetString(4),
                                BankId = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }

            return accounts;
        }

        public async Task<Accounts?> GetAccountsByIdAsync(int id)
        {
            const string query = "SELECT AccountId, AccountNumber, AccountHolderName, Balance, AccountType, BankId FROM Accounts WHERE AccountId = @AccountId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountId", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Accounts
                            {
                                AccountId = reader.GetInt32(0),
                                AccountNumber = reader.GetString(1),
                                AccountHolderName = reader.GetString(2),
                                Balance = reader.GetDecimal(3),
                                AccountType = reader.GetString(4),
                                BankId = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Accounts> AddAccountAsync(Accounts account)
        {
            const string query = @"
                INSERT INTO Accounts (AccountNumber, AccountHolderName, Balance, AccountType, BankId)
                OUTPUT INSERTED.AccountId
                VALUES (@AccountNumber, @AccountHolderName, @Balance, @AccountType, @BankId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    command.Parameters.AddWithValue("@AccountHolderName", account.AccountHolderName);
                    command.Parameters.AddWithValue("@Balance", account.Balance);
                    command.Parameters.AddWithValue("@AccountType", account.AccountType);
                    command.Parameters.AddWithValue("@BankId", account.BankId);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    account.AccountId = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            return account;
        }
    }
}
