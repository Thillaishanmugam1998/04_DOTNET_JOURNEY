using Bank_Account_API.DTOs;
using Bank_Account_API.Models;

namespace Bank_Account_API.Repositories
{
    public interface IAccountRepository
    {

        public Task<IEnumerable<Accounts>> GetAccountsAsync();

        public Task<Accounts> GetAccountsByIdAsync(int id);

        public Task<Accounts> AddAccountAsync(Accounts account);

    }
}
