using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;

namespace Bank_Account_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountsResponseDto>> GetAccountsAsync()
        {
            var accounts = await _accountRepository.GetAccountsAsync();
            return accounts.Select(a => new AccountsResponseDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountHolderName = a.AccountHolderName,
                Balance = a.Balance,
                AccountType = a.AccountType,
                BankId = a.BankId
            });
        }

        public async Task<AccountsResponseDto?> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountsByIdAsync(id);
            if (account == null)
            {
                return null;
            }

            return new AccountsResponseDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                AccountHolderName = account.AccountHolderName,
                Balance = account.Balance,
                AccountType = account.AccountType,
                BankId = account.BankId
            };
        }

        public async Task<AccountsResponseDto> AddAccountAsync(AccountCreateDto accountCreateDto)
        {
            var accountEntity = new Accounts
            {
                AccountNumber = accountCreateDto.AccountNumber,
                AccountHolderName = accountCreateDto.AccountHolderName,
                Balance = accountCreateDto.Balance,
                AccountType = accountCreateDto.AccountType,
                BankId = accountCreateDto.BankId
            };

            var addedAccount = await _accountRepository.AddAccountAsync(accountEntity);

            return new AccountsResponseDto
            {
                AccountId = addedAccount.AccountId,
                AccountNumber = addedAccount.AccountNumber,
                AccountHolderName = addedAccount.AccountHolderName,
                Balance = addedAccount.Balance,
                AccountType = addedAccount.AccountType,
                BankId = addedAccount.BankId
            };
        }
    }
}
