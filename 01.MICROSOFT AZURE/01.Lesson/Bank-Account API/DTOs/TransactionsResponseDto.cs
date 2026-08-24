namespace Bank_Account_API.DTOs
{
    public class TransactionsResponseDto
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
    }
}
