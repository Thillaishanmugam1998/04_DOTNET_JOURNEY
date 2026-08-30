using Microsoft.AspNetCore.Mvc;
using Bank_Account_API.Services;
using Bank_Account_API.DTOs;


namespace Bank_Account_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly Serilog.ILogger _logger;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
            _logger = Serilog.Log.ForContext<AccountsController>();
        }

        // GET: api/accounts
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            _logger.Information("GET /api/accounts — Fetching all accounts");

            try
            {
                var accounts = await _accountService.GetAccountsAsync();
                _logger.Information("GET /api/accounts — Returned {AccountCount} accounts", accounts.Count());
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/accounts — Failed to fetch accounts");
                return StatusCode(500, "An error occurred while fetching accounts.");
            }
        }

        // GET: api/accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            _logger.Information("GET /api/accounts/{AccountId} — Fetching account", id);

            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    _logger.Warning("GET /api/accounts/{AccountId} — Account not found", id);
                    return NotFound($"Account with ID {id} not found.");
                }

                _logger.Information("GET /api/accounts/{AccountId} — Found: {AccountHolderName}, Balance: {Balance}",
                    id, account.AccountHolderName, account.Balance);
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/accounts/{AccountId} — Failed to fetch account", id);
                return StatusCode(500, "An error occurred while fetching the account.");
            }
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountCreateDto accountCreateDto)
        {
            _logger.Information("POST /api/accounts — Creating account for: {AccountHolderName}, BankId: {BankId}, Type: {AccountType}",
                accountCreateDto.AccountHolderName, accountCreateDto.BankId, accountCreateDto.AccountType);

            if (!ModelState.IsValid)
            {
                _logger.Warning("POST /api/accounts — Validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var createdAccount = await _accountService.AddAccountAsync(accountCreateDto);
                _logger.Information("POST /api/accounts — Account created: ID {AccountId}, AccountNo: {AccountNumber}",
                    createdAccount.AccountId, createdAccount.AccountNumber);
                return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId }, createdAccount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "POST /api/accounts — Failed to create account for: {AccountHolderName}",
                    accountCreateDto.AccountHolderName);
                return StatusCode(500, "An error occurred while creating the account.");
            }
        }
    }
}
