using Microsoft.AspNetCore.Mvc;
using Bank_Account_API.Services;
using Bank_Account_API.DTOs;


namespace Bank_Account_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;
        private readonly Serilog.ILogger _logger;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
            _logger = Serilog.Log.ForContext<TransactionsController>();
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            _logger.Information("GET /api/transactions — Fetching all transactions");

            try
            {
                var transactions = await _transactionsService.GetAllTransactionsAsync();
                _logger.Information("GET /api/transactions — Returned {TransactionCount} transactions", transactions.Count());
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/transactions — Failed to fetch transactions");
                return StatusCode(500, "An error occurred while fetching transactions.");
            }
        }

        // GET: api/transactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            _logger.Information("GET /api/transactions/{TransactionId} — Fetching transaction", id);

            try
            {
                var transaction = await _transactionsService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    _logger.Warning("GET /api/transactions/{TransactionId} — Transaction not found", id);
                    return NotFound($"Transaction with ID {id} not found.");
                }

                _logger.Information("GET /api/transactions/{TransactionId} — Found: Type: {TransactionType}, Amount: {Amount}",
                    id, transaction.TransactionType, transaction.Amount);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/transactions/{TransactionId} — Failed to fetch transaction", id);
                return StatusCode(500, "An error occurred while fetching the transaction.");
            }
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionCreateDto transactionCreateDto)
        {
            _logger.Information("POST /api/transactions — Creating transaction: AccountId: {AccountId}, Type: {TransactionType}, Amount: {Amount}",
                transactionCreateDto.AccountId, transactionCreateDto.TransactionType, transactionCreateDto.Amount);

            if (!ModelState.IsValid)
            {
                _logger.Warning("POST /api/transactions — Validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var createdTransaction = await _transactionsService.AddTransactionAsync(transactionCreateDto);
                _logger.Information("POST /api/transactions — Transaction created: ID {TransactionId}, Amount: {Amount}, Date: {TransactionDate}",
                    createdTransaction.TransactionId, createdTransaction.Amount, createdTransaction.TransactionDate);
                return CreatedAtAction(nameof(GetTransactionById), new { id = createdTransaction.TransactionId }, createdTransaction);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "POST /api/transactions — Failed to create transaction for AccountId: {AccountId}",
                    transactionCreateDto.AccountId);
                return StatusCode(500, "An error occurred while creating the transaction.");
            }
        }
    }
}
