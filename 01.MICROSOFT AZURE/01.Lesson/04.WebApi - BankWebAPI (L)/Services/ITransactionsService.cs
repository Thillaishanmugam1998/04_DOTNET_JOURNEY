using Bank_Account_API.DTOs;

namespace Bank_Account_API.Services
{
    public interface ITransactionsService
    {
        Task<IEnumerable<TransactionsResponseDto>> GetAllTransactionsAsync();
        Task<TransactionsResponseDto?> GetTransactionByIdAsync(int id);
        Task<TransactionsResponseDto> AddTransactionAsync(TransactionCreateDto transactionCreateDto);
    }
}
