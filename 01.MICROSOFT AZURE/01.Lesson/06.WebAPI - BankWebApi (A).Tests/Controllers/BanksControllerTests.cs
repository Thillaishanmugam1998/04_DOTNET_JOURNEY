using Moq;
using Microsoft.AspNetCore.Mvc;
using Bank_Account_API.Controllers;
using Bank_Account_API.Services;
using Bank_Account_API.DTOs;

namespace Bank_Account_API.Tests.Controllers
{
    // ═══════════════════════════════════════════════════════════════
    //  BANKS CONTROLLER — UNIT TESTS
    //  Test Target  : BanksController (Controller Layer)
    //  Mock         : IBankService (Service Layer)
    //  Focus        : HTTP Status Codes (200, 201, 404, 500)
    // ═══════════════════════════════════════════════════════════════

    public class BanksControllerTests
    {
        private readonly Mock<IBankService> _mockBankService;
        private readonly BanksController _controller;

        public BanksControllerTests()
        {
            _mockBankService = new Mock<IBankService>();
            _controller = new BanksController(_mockBankService.Object);
        }


        // ╔═══════════════════════════════════════════════════════════╗
        // ║  TEST GROUP 1: GetAllBanks                               ║
        // ╚═══════════════════════════════════════════════════════════╝

        #region GetAllBanks Tests

        [Fact]
        public async Task GetAllBanks_WhenBanksExist_ReturnsOkWithBankList()
        {
            // ── ARRANGE ──
            var fakeBanks = new List<BankResponseDto>
            {
                new BankResponseDto
                {
                    BankId = 1, BankName = "SBI",
                    BranchCode = "SBI001", Address = "Chennai",
                    IFSCCode = "SBIN0001234"
                },
                new BankResponseDto
                {
                    BankId = 2, BankName = "HDFC",
                    BranchCode = "HDFC001", Address = "Mumbai",
                    IFSCCode = "HDFC0005678"
                }
            };

            _mockBankService
                .Setup(service => service.GetAllBanksAsync())
                .ReturnsAsync(fakeBanks);

            // ── ACT ──
            var result = await _controller.GetAllBanks();

            // ── ASSERT ──
            var okResult = Assert.IsType<OkObjectResult>(result);          // 200 OK aa?
            Assert.Equal(200, okResult.StatusCode);                         // Status code 200 aa?
            var banks = Assert.IsAssignableFrom<IEnumerable<BankResponseDto>>(okResult.Value);
            Assert.Equal(2, banks.Count());                                 // 2 banks return aachaa?
        }

        [Fact]
        public async Task GetAllBanks_WhenServiceThrowsException_Returns500()
        {
            // ── ARRANGE ──
            // Service layer la exception throw aana, controller 500 return pannum
            _mockBankService
                .Setup(service => service.GetAllBanksAsync())
                .ThrowsAsync(new Exception("Database connection failed"));

            // ── ACT ──
            var result = await _controller.GetAllBanks();

            // ── ASSERT ──
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);                     // 500 Internal Server Error aa?
        }

        #endregion


        // ╔═══════════════════════════════════════════════════════════╗
        // ║  TEST GROUP 2: GetBankById                                ║
        // ╚═══════════════════════════════════════════════════════════╝

        #region GetBankById Tests

        [Fact]
        public async Task GetBankById_WhenBankExists_ReturnsOkWithBank()
        {
            // ── ARRANGE ──
            var fakeBank = new BankResponseDto
            {
                BankId = 1, BankName = "SBI",
                BranchCode = "SBI001", Address = "Chennai",
                IFSCCode = "SBIN0001234"
            };

            _mockBankService
                .Setup(service => service.GetBankByIdAsync(1))
                .ReturnsAsync(fakeBank);

            // ── ACT ──
            var result = await _controller.GetBankById(1);

            // ── ASSERT ──
            var okResult = Assert.IsType<OkObjectResult>(result);          // 200 OK aa?
            var bank = Assert.IsType<BankResponseDto>(okResult.Value);
            Assert.Equal("SBI", bank.BankName);
        }

        [Fact]
        public async Task GetBankById_WhenBankNotFound_ReturnsNotFound()
        {
            // ── ARRANGE ──
            _mockBankService
                .Setup(service => service.GetBankByIdAsync(999))
                .ReturnsAsync((BankResponseDto?)null);

            // ── ACT ──
            var result = await _controller.GetBankById(999);

            // ── ASSERT ──
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);  // 404 aa?
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetBankById_WhenServiceThrowsException_Returns500()
        {
            // ── ARRANGE ──
            _mockBankService
                .Setup(service => service.GetBankByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // ── ACT ──
            var result = await _controller.GetBankById(1);

            // ── ASSERT ──
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion


        // ╔═══════════════════════════════════════════════════════════╗
        // ║  TEST GROUP 3: AddBank                                    ║
        // ╚═══════════════════════════════════════════════════════════╝

        #region AddBank Tests

        [Fact]
        public async Task AddBank_ValidBank_ReturnsCreatedAtAction()
        {
            // ── ARRANGE ──
            var bankCreateDto = new BankCreateDto
            {
                BankName = "Axis Bank",
                BranchCode = "AXIS001",
                Address = "Bangalore",
                IFSCCode = "UTIB0001234"
            };

            var createdBank = new BankResponseDto
            {
                BankId = 5, BankName = "Axis Bank",
                BranchCode = "AXIS001", Address = "Bangalore",
                IFSCCode = "UTIB0001234"
            };

            _mockBankService
                .Setup(service => service.AddBankAsync(bankCreateDto))
                .ReturnsAsync(createdBank);

            // ── ACT ──
            var result = await _controller.AddBank(bankCreateDto);

            // ── ASSERT ──
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);   // 201 Created aa?
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(nameof(BanksController.GetBankById), createdResult.ActionName);

            var bank = Assert.IsType<BankResponseDto>(createdResult.Value);
            Assert.Equal(5, bank.BankId);
            Assert.Equal("Axis Bank", bank.BankName);
        }

        [Fact]
        public async Task AddBank_InvalidModel_ReturnsBadRequest()
        {
            // ── ARRANGE ──
            // ModelState la error add panrom — validation failure simulate
            _controller.ModelState.AddModelError("BankName", "Bank name is required");

            var bankCreateDto = new BankCreateDto
            {
                BankName = "",  // Invalid — empty name
                BranchCode = "BR001",
                Address = "Test",
                IFSCCode = "TEST0001234"
            };

            // ── ACT ──
            var result = await _controller.AddBank(bankCreateDto);

            // ── ASSERT ──
            Assert.IsType<BadRequestObjectResult>(result);   // 400 Bad Request aa?
        }

        [Fact]
        public async Task AddBank_WhenServiceThrowsException_Returns500()
        {
            // ── ARRANGE ──
            var bankCreateDto = new BankCreateDto
            {
                BankName = "Test Bank",
                BranchCode = "TB001",
                Address = "Test",
                IFSCCode = "TEST0001234"
            };

            _mockBankService
                .Setup(service => service.AddBankAsync(It.IsAny<BankCreateDto>()))
                .ThrowsAsync(new Exception("Database save failed"));

            // ── ACT ──
            var result = await _controller.AddBank(bankCreateDto);

            // ── ASSERT ──
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        #endregion
    }
}
