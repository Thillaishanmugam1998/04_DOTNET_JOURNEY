using System.ComponentModel.DataAnnotations;

namespace Bank_Account_API.DTOs
{
    public class TransactionCreateDto
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [RegularExpression("^(Credit|Debit)$", ErrorMessage = "TransactionType must be Credit or Debit" )]
        public string TransactionType { get; set; } = null!;

        [Range(0.01,double.MaxValue,ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [StringLength( 200,ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }
}
