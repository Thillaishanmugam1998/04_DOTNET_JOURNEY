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

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }

        // GET: api/banks
        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var banks = await _bankService.GetAllBanksAsync();
            return Ok(banks);
        }

        // GET: api/banks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankById(int id)
        {
            var bank = await _bankService.GetBankByIdAsync(id);
            if (bank == null)
            {
                return NotFound($"Bank with ID {id} not found.");
            }
            return Ok(bank);
        }

        // POST: api/banks
        [HttpPost]
        public async Task<IActionResult> AddBank([FromBody] BankCreateDto bankCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBank = await _bankService.AddBankAsync(bankCreateDto);
            return CreatedAtAction(nameof(GetBankById), new { id = createdBank.BankId }, createdBank);
        }
    }
}
