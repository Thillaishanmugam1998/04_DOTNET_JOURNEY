using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;

namespace Bank_Account_API.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<IEnumerable<TransactionsResponseDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionsRepository.GetAllTransactionsAsync();
            return transactions.Select(t => new TransactionsResponseDto
            {
                TransactionId = t.TransactionId,
                AccountId = t.AccountId,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Description = t.Description
            });
        }

        public async Task<TransactionsResponseDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionsRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return null;
            }

            return new TransactionsResponseDto
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description
            };
        }

        public async Task<TransactionsResponseDto> AddTransactionAsync(TransactionCreateDto transactionCreateDto)
        {
            var transactionEntity = new Transactions
            {
                AccountId = transactionCreateDto.AccountId,
                TransactionType = transactionCreateDto.TransactionType,
                Amount = transactionCreateDto.Amount,
                Description = transactionCreateDto.Description,
                TransactionDate = DateTime.UtcNow
            };

            var addedTransaction = await _transactionsRepository.AddTransactionAsync(transactionEntity);

            return new TransactionsResponseDto
            {
                TransactionId = addedTransaction.TransactionId,
                AccountId = addedTransaction.AccountId,
                TransactionType = addedTransaction.TransactionType,
                Amount = addedTransaction.Amount,
                TransactionDate = addedTransaction.TransactionDate,
                Description = addedTransaction.Description
            };
        }
    }
}
