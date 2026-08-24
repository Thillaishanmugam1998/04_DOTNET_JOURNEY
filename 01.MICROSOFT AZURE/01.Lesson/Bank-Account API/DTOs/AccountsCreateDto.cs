using System.ComponentModel.DataAnnotations;

namespace Bank_Account_API.DTOs
{
    public class AccountCreateDto
    {
        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
        public decimal Balance { get; set; }

        [Required]
        [RegularExpression("^(Savings|Current)$", ErrorMessage = "AccountType must be Savings or Current")]
        public string AccountType { get; set; } = null!;

        [Required]
        public int BankId { get; set; }
    }
}