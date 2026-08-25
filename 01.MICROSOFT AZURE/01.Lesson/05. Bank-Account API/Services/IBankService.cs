using Bank_Account_API.DTOs;

namespace Bank_Account_API.Services
{
    public interface IBankService
    {
        Task<IEnumerable<BankResponseDto>> GetAllBanksAsync();
        Task<BankResponseDto?> GetBankByIdAsync(int id);
        Task<BankResponseDto> AddBankAsync(BankCreateDto bankCreateDto);
    }
}
