namespace Bank_Account_API.DTOs
{
    public class AccountsResponseDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string AccountHolderName { get; set; } = null!;
        public decimal Balance { get; set; }
        public string AccountType { get; set; } = null!;
        public int BankId { get; set; }
    }
}
