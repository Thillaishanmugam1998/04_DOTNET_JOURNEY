using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Account_API.Models
{
    public class Transactions
    {
        // TABLE OVERVIEW:
        // Transactions – TransactionId, AccountId(FK → Accounts), TransactionType(Credit/Debit), Amount, TransactionDate, Description
        // This is called Transaction Entity Class. It represents the Transactions table in the database.
        
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; } = null!;   // Credit / Debit

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? Description { get; set; }

        // ---- Foreign Key ----
        [ForeignKey("Accounts")]
        public int AccountId { get; set; }

        // ---- Navigation Property (Many Transactions → One Account) ----
        public Accounts Accounts { get; set; } = null!;
    }
}
