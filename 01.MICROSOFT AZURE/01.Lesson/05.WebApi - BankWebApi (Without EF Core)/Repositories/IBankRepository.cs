using Bank_Account_API.Models;

namespace Bank_Account_API.Repositories
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAllBanksAsync();
        Task<Bank?> GetBankByIdAsync(int id);
        Task<Bank> AddBankAsync(Bank bank);
    }
}
