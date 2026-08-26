using Bank_Account_API.Data;
using Bank_Account_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Account_API.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext _context;

        public BankRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bank>> GetAllBanksAsync()
        {
            return await _context.Banks.ToListAsync();
        }

        public async Task<Bank?> GetBankByIdAsync(int id)
        {
            return await _context.Banks.FindAsync(id);
        }

        public async Task<Bank> AddBankAsync(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();
            return bank;
        }
    }
}
