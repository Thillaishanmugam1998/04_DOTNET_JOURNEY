using Microsoft.AspNetCore.Mvc;
using Bank_Account_API.Services;
using Bank_Account_API.DTOs;


namespace Bank_Account_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly Serilog.ILogger _logger;

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
            _logger = Serilog.Log.ForContext<BanksController>();
        }

        // GET: api/banks
        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            _logger.Information("GET /api/banks — Fetching all banks");

            try
            {
                var banks = await _bankService.GetAllBanksAsync();
                _logger.Information("GET /api/banks — Returned {BankCount} banks", banks.Count());
                return Ok(banks);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/banks — Failed to fetch banks");
                return StatusCode(500, "An error occurred while fetching banks.");
            }
        }

        // GET: api/banks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankById(int id)
        {
            _logger.Information("GET /api/banks/{BankId} — Fetching bank", id);

            try
            {
                var bank = await _bankService.GetBankByIdAsync(id);
                if (bank == null)
                {
                    _logger.Warning("GET /api/banks/{BankId} — Bank not found", id);
                    return NotFound($"Bank with ID {id} not found.");
                }

                _logger.Information("GET /api/banks/{BankId} — Found bank: {BankName}", id, bank.BankName);
                return Ok(bank);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GET /api/banks/{BankId} — Failed to fetch bank", id);
                return StatusCode(500, "An error occurred while fetching the bank.");
            }
        }

        // POST: api/banks
        [HttpPost]
        public async Task<IActionResult> AddBank([FromBody] BankCreateDto bankCreateDto)
        {
            _logger.Information("POST /api/banks — Creating bank: {BankName}, IFSC: {IFSCCode}",
                bankCreateDto.BankName, bankCreateDto.IFSCCode);

            if (!ModelState.IsValid)
            {
                _logger.Warning("POST /api/banks — Validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var createdBank = await _bankService.AddBankAsync(bankCreateDto);
                _logger.Information("POST /api/banks — Bank created successfully with ID {BankId}", createdBank.BankId);
                return CreatedAtAction(nameof(GetBankById), new { id = createdBank.BankId }, createdBank);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "POST /api/banks — Failed to create bank: {BankName}", bankCreateDto.BankName);
                return StatusCode(500, "An error occurred while creating the bank.");
            }
        }
    }
}
