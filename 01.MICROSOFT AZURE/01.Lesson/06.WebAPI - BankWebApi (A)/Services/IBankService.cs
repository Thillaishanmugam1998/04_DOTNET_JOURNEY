using Bank_Account_API.DTOs;

namespace Bank_Account_API.Services
{
    public interface IBankService
    {
        public Task<IEnumerable<BankResponseDto>> GetAllBanksAsync();
        public Task<BankResponseDto?> GetBankByIdAsync(int id);
        public Task<BankResponseDto> AddBankAsync(BankCreateDto bankCreateDto);
    }
}
