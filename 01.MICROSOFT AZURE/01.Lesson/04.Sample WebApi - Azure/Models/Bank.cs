using System.ComponentModel.DataAnnotations;

namespace Bank_Account_API.Models
{
    public class Bank
    {
        // TABLE OVERVIEW:
        // Banks – BankId, BankName, BranchCode, Address, IFSCCode
        // This is called Bank Entity Class. It represents the Banks table in the database.

        public int BankId { get; set; }

        [Required]
        [StringLength(250)]
        public string BankName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string BranchCode { get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(11)]
        public string IFSCCode { get; set; } = null!;

        // ---- Navigation Property (One Bank → Many Accounts) ----
        public ICollection<Accounts> Accounts { get; set; } = new List<Accounts>();
    }
}