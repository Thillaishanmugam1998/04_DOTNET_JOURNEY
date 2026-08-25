using Bank_Account_API.Models;
using Microsoft.Data.SqlClient;

namespace Bank_Account_API.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly string _connectionString;

        public TransactionsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection is not configured.");
        }

        public async Task<IEnumerable<Transactions>> GetAllTransactionsAsync()
        {
            var transactions = new List<Transactions>();
            const string query = "SELECT TransactionId, AccountId, TransactionType, Amount, TransactionDate, Description FROM Transactions";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transactions.Add(new Transactions
                            {
                                TransactionId = reader.GetInt32(0),
                                AccountId = reader.GetInt32(1),
                                TransactionType = reader.GetString(2),
                                Amount = reader.GetDecimal(3),
                                TransactionDate = reader.GetDateTime(4),
                                Description = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return transactions;
        }

        public async Task<Transactions?> GetTransactionByIdAsync(int id)
        {
            const string query = "SELECT TransactionId, AccountId, TransactionType, Amount, TransactionDate, Description FROM Transactions WHERE TransactionId = @TransactionId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransactionId", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Transactions
                            {
                                TransactionId = reader.GetInt32(0),
                                AccountId = reader.GetInt32(1),
                                TransactionType = reader.GetString(2),
                                Amount = reader.GetDecimal(3),
                                TransactionDate = reader.GetDateTime(4),
                                Description = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Transactions> AddTransactionAsync(Transactions transaction)
        {
            const string query = @"
                INSERT INTO Transactions (AccountId, TransactionType, Amount, TransactionDate, Description)
                OUTPUT INSERTED.TransactionId
                VALUES (@AccountId, @TransactionType, @Amount, @TransactionDate, @Description)";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                    command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
                    command.Parameters.AddWithValue("@Description", (object?)transaction.Description ?? DBNull.Value);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    transaction.TransactionId = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            return transaction;
        }
    }
}
