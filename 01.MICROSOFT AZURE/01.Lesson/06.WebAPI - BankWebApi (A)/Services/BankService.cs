using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;


namespace Bank_Account_API.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly Serilog.ILogger _logger;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
            _logger = Serilog.Log.ForContext<BankService>();
        }

        public async Task<IEnumerable<BankResponseDto>> GetAllBanksAsync()
        {
            _logger.Information("BankService.GetAllBanksAsync — Fetching all banks from repository");

            var banks = await _bankRepository.GetAllBanksAsync();
            var bankList = banks.Select(b => new BankResponseDto
            {
                BankId = b.BankId,
                BankName = b.BankName,
                BranchCode = b.BranchCode,
                Address = b.Address,
                IFSCCode = b.IFSCCode
            });

            _logger.Debug("BankService.GetAllBanksAsync — Mapped {BankCount} banks to DTOs", bankList.Count());
            return bankList;
        }

        public async Task<BankResponseDto?> GetBankByIdAsync(int id)
        {
            _logger.Information("BankService.GetBankByIdAsync — Looking up BankId: {BankId}", id);

            var bank = await _bankRepository.GetBankByIdAsync(id);
            if (bank == null)
            {
                _logger.Warning("BankService.GetBankByIdAsync — BankId: {BankId} not found in database", id);
                return null;
            }

            _logger.Debug("BankService.GetBankByIdAsync — Found Bank: {BankName}, IFSC: {IFSCCode}", bank.BankName, bank.IFSCCode);
            return new BankResponseDto
            {
                BankId = bank.BankId,
                BankName = bank.BankName,
                BranchCode = bank.BranchCode,
                Address = bank.Address,
                IFSCCode = bank.IFSCCode
            };
        }

        public async Task<BankResponseDto> AddBankAsync(BankCreateDto bankCreateDto)
        {
            _logger.Information("BankService.AddBankAsync — Adding bank: {BankName}, Branch: {BranchCode}",
                bankCreateDto.BankName, bankCreateDto.BranchCode);

            var bankEntity = new Bank
            {
                BankName = bankCreateDto.BankName,
                BranchCode = bankCreateDto.BranchCode,
                Address = bankCreateDto.Address,
                IFSCCode = bankCreateDto.IFSCCode
            };

            var addedBank = await _bankRepository.AddBankAsync(bankEntity);

            _logger.Information("BankService.AddBankAsync — Bank saved to DB with BankId: {BankId}", addedBank.BankId);
            return new BankResponseDto
            {
                BankId = addedBank.BankId,
                BankName = addedBank.BankName,
                BranchCode = addedBank.BranchCode,
                Address = addedBank.Address,
                IFSCCode = addedBank.IFSCCode
            };
        }
    }
}
