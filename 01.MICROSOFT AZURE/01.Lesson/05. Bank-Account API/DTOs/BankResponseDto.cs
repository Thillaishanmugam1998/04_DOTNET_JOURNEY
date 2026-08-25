namespace Bank_Account_API.DTOs
{
    public class BankResponseDto
    {
        public int BankId { get; set; }
        public string BankName { get; set; } = null!;
        public string BranchCode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IFSCCode { get; set; } = null!;
    }
}
