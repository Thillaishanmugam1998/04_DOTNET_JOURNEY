using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_Account_API.Models
{
    public class Accounts
    {
        // TABLE OVERVIEW:
        // Accounts – AccountId, AccountNumber, AccountHolderName, Balance, AccountType, BankId(FK → Banks)
        // This is called Account Entity Class. It represents the Accounts table in the database.

        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = null!;

        // ---- Foreign Key (Many Accounts → One Bank) ----
        [ForeignKey("Bank")]
        public int BankId { get; set; }
        public Bank Bank { get; set; } = null!;

        // ---- Navigation Property (One Account → Many Transactions) ----
        public ICollection<Transactions> Transactions { get; set; } = new List<Transactions>();
    }
}