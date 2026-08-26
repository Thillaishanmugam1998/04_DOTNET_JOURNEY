using Bank_Account_API.DTOs;

namespace Bank_Account_API.Services
{
    public interface IAccountService
    {
        public Task<IEnumerable<AccountsResponseDto>> GetAccountsAsync();
        public Task<AccountsResponseDto?> GetAccountByIdAsync(int id);
        public Task<AccountsResponseDto> AddAccountAsync(AccountCreateDto accountCreateDto);
    }
}
