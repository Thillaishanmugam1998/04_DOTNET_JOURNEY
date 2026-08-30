using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;


namespace Bank_Account_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly Serilog.ILogger _logger;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _logger = Serilog.Log.ForContext<AccountService>();
        }

        public async Task<IEnumerable<AccountsResponseDto>> GetAccountsAsync()
        {
            _logger.Information("AccountService.GetAccountsAsync — Fetching all accounts from repository");

            var accounts = await _accountRepository.GetAccountsAsync();
            var accountList = accounts.Select(a => new AccountsResponseDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountHolderName = a.AccountHolderName,
                Balance = a.Balance,
                AccountType = a.AccountType,
                BankId = a.BankId
            });

            _logger.Debug("AccountService.GetAccountsAsync — Mapped {AccountCount} accounts to DTOs", accountList.Count());
            return accountList;
        }

        public async Task<AccountsResponseDto?> GetAccountByIdAsync(int id)
        {
            _logger.Information("AccountService.GetAccountByIdAsync — Looking up AccountId: {AccountId}", id);

            var account = await _accountRepository.GetAccountsByIdAsync(id);
            if (account == null)
            {
                _logger.Warning("AccountService.GetAccountByIdAsync — AccountId: {AccountId} not found in database", id);
                return null;
            }

            _logger.Debug("AccountService.GetAccountByIdAsync — Found Account: {AccountHolderName}, Balance: {Balance}",
                account.AccountHolderName, account.Balance);
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
            _logger.Information("AccountService.AddAccountAsync — Creating account for: {AccountHolderName}, Type: {AccountType}, BankId: {BankId}",
                accountCreateDto.AccountHolderName, accountCreateDto.AccountType, accountCreateDto.BankId);

            var accountEntity = new Accounts
            {
                AccountNumber = accountCreateDto.AccountNumber,
                AccountHolderName = accountCreateDto.AccountHolderName,
                Balance = accountCreateDto.Balance,
                AccountType = accountCreateDto.AccountType,
                BankId = accountCreateDto.BankId
            };

            var addedAccount = await _accountRepository.AddAccountAsync(accountEntity);

            _logger.Information("AccountService.AddAccountAsync — Account saved to DB: AccountId: {AccountId}, AccountNo: {AccountNumber}",
                addedAccount.AccountId, addedAccount.AccountNumber);
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
