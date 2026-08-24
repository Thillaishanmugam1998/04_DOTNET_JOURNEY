using System.ComponentModel.DataAnnotations;

namespace Bank_Account_API.DTOs
{
    public class BankCreateDto
    {
        [Required(ErrorMessage = "Bank name is required")]
        [StringLength(250, ErrorMessage = "Bank name cannot exceed 250 characters")]
        public string BankName { get; set; } = null!;

        [Required(ErrorMessage = "Branch code is required")]
        [StringLength(100, ErrorMessage = "Branch code cannot exceed 100 characters")]
        public string BranchCode { get; set; } = null!;
        
        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; } = null!;
        
        [Required(ErrorMessage = "IFSC code is required")]
        [StringLength(11, ErrorMessage = "IFSC code must be exactly 11 characters")]
        public string IFSCCode { get; set; } = null!;

    }
}
