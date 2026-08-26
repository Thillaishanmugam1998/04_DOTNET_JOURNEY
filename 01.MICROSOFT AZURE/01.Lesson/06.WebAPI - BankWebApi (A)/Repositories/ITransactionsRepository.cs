using Bank_Account_API.Models;

namespace Bank_Account_API.Repositories
{
    public interface ITransactionsRepository
    {
        Task<IEnumerable<Transactions>> GetAllTransactionsAsync();
        Task<Transactions?> GetTransactionByIdAsync(int id);
        Task<Transactions> AddTransactionAsync(Transactions transaction);
    }
}
