using Microsoft.EntityFrameworkCore;
using Bank_Account_API.Models;

namespace Bank_Account_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region --- 01.CONSTRUCTOR ---
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        #endregion

        #region --- 02.DBSET PROPERTIES ---

        //Banks - Table Name
        //Bank - Entity Class Name

        public DbSet<Bank> Banks { get; set; } = null!;
        public DbSet<Accounts> Accounts { get; set; } = null!;
        public DbSet<Transactions> Transactions { get; set; } = null!;
        #endregion

        #region --- 03.OVERRIDING ONMODEL CREATING METHOD ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed 5 initial banks (Seeding cannot be done inside Model classes)
            modelBuilder.Entity<Bank>().HasData(
                new Bank { BankId = 1, BankName = "State Bank of India", BranchCode = "SBI001", Address = "Mumbai, Maharashtra", IFSCCode = "SBIN0000001" },
                new Bank { BankId = 2, BankName = "HDFC Bank", BranchCode = "HDFC01", Address = "Mumbai, Maharashtra", IFSCCode = "HDFC0000001" },
                new Bank { BankId = 3, BankName = "ICICI Bank", BranchCode = "ICICI1", Address = "Mumbai, Maharashtra", IFSCCode = "ICIC0000001" },
                new Bank { BankId = 4, BankName = "Axis Bank", BranchCode = "AXIS01", Address = "Mumbai, Maharashtra", IFSCCode = "UTIB0000001" },
                new Bank { BankId = 5, BankName = "Punjab National Bank", BranchCode = "PNB001", Address = "New Delhi, Delhi", IFSCCode = "PUNB0000001" }
            );


            // Seed Accounts
            modelBuilder.Entity<Accounts>().HasData(
                new Accounts { AccountId = 1, AccountNumber = "100010001001", AccountHolderName = "Thillai Shanmugam", Balance = 50000.00m, AccountType = "Savings", BankId = 1 },
                new Accounts { AccountId = 2, AccountNumber = "200020002002", AccountHolderName = "Tamilvani", Balance = 75000.00m, AccountType = "Savings", BankId = 2 },
                new Accounts { AccountId = 3, AccountNumber = "300030003003", AccountHolderName = "Dharshini", Balance = 60000.00m, AccountType = "Current", BankId = 3 },
                new Accounts { AccountId = 4, AccountNumber = "400040004004", AccountHolderName = "Abinaya", Balance = 90000.00m, AccountType = "Savings", BankId = 4 }
            );

            // Seed Transactions
            modelBuilder.Entity<Transactions>().HasData(
                new Transactions { TransactionId = 1, AccountId = 1, TransactionType = "Credit", Amount = 10000.00m, TransactionDate = new DateTime(2026, 8, 20, 10, 0, 0, DateTimeKind.Utc), Description = "Initial Deposit" },
                new Transactions { TransactionId = 2, AccountId = 1, TransactionType = "Debit", Amount = 2000.00m, TransactionDate = new DateTime(2026, 8, 22, 14, 30, 0, DateTimeKind.Utc), Description = "ATM Withdrawal" },
                new Transactions { TransactionId = 3, AccountId = 2, TransactionType = "Credit", Amount = 15000.00m, TransactionDate = new DateTime(2026, 8, 21, 11, 15, 0, DateTimeKind.Utc), Description = "Salary Credit" },
                new Transactions { TransactionId = 4, AccountId = 3, TransactionType = "Debit", Amount = 5000.00m, TransactionDate = new DateTime(2026, 8, 23, 9, 45, 0, DateTimeKind.Utc), Description = "Online Shopping" },
                new Transactions { TransactionId = 5, AccountId = 4, TransactionType = "Credit", Amount = 25000.00m, TransactionDate = new DateTime(2026, 8, 24, 16, 0, 0, DateTimeKind.Utc), Description = "Funds Transfer" }
            );
        }
        #endregion

    }
}
