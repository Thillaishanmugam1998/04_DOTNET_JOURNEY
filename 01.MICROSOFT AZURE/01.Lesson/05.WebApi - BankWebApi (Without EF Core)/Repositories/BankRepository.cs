using Bank_Account_API.Models;
using Microsoft.Data.SqlClient;

namespace Bank_Account_API.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly string _connectionString;

        public BankRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection is not configured.");
        }

        public async Task<IEnumerable<Bank>> GetAllBanksAsync()
        {
            var banks = new List<Bank>();
            const string query = "SELECT BankId, BankName, BranchCode, Address, IFSCCode FROM Banks";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            banks.Add(new Bank
                            {
                                BankId = reader.GetInt32(0),
                                BankName = reader.GetString(1),
                                BranchCode = reader.GetString(2),
                                Address = reader.GetString(3),
                                IFSCCode = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return banks;
        }

        public async Task<Bank?> GetBankByIdAsync(int id)
        {
            const string query = "SELECT BankId, BankName, BranchCode, Address, IFSCCode FROM Banks WHERE BankId = @BankId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BankId", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Bank
                            {
                                BankId = reader.GetInt32(0),
                                BankName = reader.GetString(1),
                                BranchCode = reader.GetString(2),
                                Address = reader.GetString(3),
                                IFSCCode = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Bank> AddBankAsync(Bank bank)
        {
            const string query = @"
                INSERT INTO Banks (BankName, BranchCode, Address, IFSCCode)
                OUTPUT INSERTED.BankId
                VALUES (@BankName, @BranchCode, @Address, @IFSCCode)";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BankName", bank.BankName);
                    command.Parameters.AddWithValue("@BranchCode", bank.BranchCode);
                    command.Parameters.AddWithValue("@Address", bank.Address);
                    command.Parameters.AddWithValue("@IFSCCode", bank.IFSCCode);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    bank.BankId = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            return bank;
        }
    }
}
