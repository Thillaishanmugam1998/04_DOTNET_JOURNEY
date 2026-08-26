using Bank_Account_API.Data;
using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Account_API.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable <Accounts>> GetAccountsAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return accounts;
        }

        public async Task<Accounts?> GetAccountsByIdAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return account;
        }

        public async Task<Accounts> AddAccountAsync(Accounts account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }
    }
}


