using Bank_Account_API.DTOs;
using Bank_Account_API.Models;
using Bank_Account_API.Repositories;

namespace Bank_Account_API.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<IEnumerable<BankResponseDto>> GetAllBanksAsync()
        {
            var banks = await _bankRepository.GetAllBanksAsync();
            return banks.Select(b => new BankResponseDto
            {
                BankId = b.BankId,
                BankName = b.BankName,
                BranchCode = b.BranchCode,
                Address = b.Address,
                IFSCCode = b.IFSCCode
            });
        }

        public async Task<BankResponseDto?> GetBankByIdAsync(int id)
        {
            var bank = await _bankRepository.GetBankByIdAsync(id);
            if (bank == null)
            {
                return null;
            }

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
            var bankEntity = new Bank
            {
                BankName = bankCreateDto.BankName,
                BranchCode = bankCreateDto.BranchCode,
                Address = bankCreateDto.Address,
                IFSCCode = bankCreateDto.IFSCCode
            };

            var addedBank = await _bankRepository.AddBankAsync(bankEntity);

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
