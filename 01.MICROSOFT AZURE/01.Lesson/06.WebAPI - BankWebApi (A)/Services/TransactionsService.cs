using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;


namespace Bank_Account_API.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly Serilog.ILogger _logger;

        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
            _logger = Serilog.Log.ForContext<TransactionsService>();
        }

        public async Task<IEnumerable<TransactionsResponseDto>> GetAllTransactionsAsync()
        {
            _logger.Information("TransactionsService.GetAllTransactionsAsync — Fetching all transactions from repository");

            var transactions = await _transactionsRepository.GetAllTransactionsAsync();
            var transactionList = transactions.Select(t => new TransactionsResponseDto
            {
                TransactionId = t.TransactionId,
                AccountId = t.AccountId,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Description = t.Description
            });

            _logger.Debug("TransactionsService.GetAllTransactionsAsync — Mapped {TransactionCount} transactions to DTOs", transactionList.Count());
            return transactionList;
        }

        public async Task<TransactionsResponseDto?> GetTransactionByIdAsync(int id)
        {
            _logger.Information("TransactionsService.GetTransactionByIdAsync — Looking up TransactionId: {TransactionId}", id);

            var transaction = await _transactionsRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                _logger.Warning("TransactionsService.GetTransactionByIdAsync — TransactionId: {TransactionId} not found in database", id);
                return null;
            }

            _logger.Debug("TransactionsService.GetTransactionByIdAsync — Found: Type: {TransactionType}, Amount: {Amount}",
                transaction.TransactionType, transaction.Amount);
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
            _logger.Information("TransactionsService.AddTransactionAsync — Creating transaction: AccountId: {AccountId}, Type: {TransactionType}, Amount: {Amount}",
                transactionCreateDto.AccountId, transactionCreateDto.TransactionType, transactionCreateDto.Amount);

            var transactionEntity = new Transactions
            {
                AccountId = transactionCreateDto.AccountId,
                TransactionType = transactionCreateDto.TransactionType,
                Amount = transactionCreateDto.Amount,
                Description = transactionCreateDto.Description,
                TransactionDate = DateTime.UtcNow
            };

            var addedTransaction = await _transactionsRepository.AddTransactionAsync(transactionEntity);

            _logger.Information("TransactionsService.AddTransactionAsync — Transaction saved to DB: TransactionId: {TransactionId}, Amount: {Amount}, Date: {TransactionDate}",
                addedTransaction.TransactionId, addedTransaction.Amount, addedTransaction.TransactionDate);
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
