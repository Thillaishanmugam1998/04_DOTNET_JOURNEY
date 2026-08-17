using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace ProductManagementApi.DTOs
{
    public class ProductUpdateDto
    {
        
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product Name mustbe between 2 and 100 characters.")]
        public string Name { get; set; } = null!;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500characters.")]
        public string? Description { get; set; }
        
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0 andcannot exceed 10,00,000.")]
        public decimal Price { get; set; }
        
        [Range(0, 100000, ErrorMessage = "Stock Quantity must be between 0 and100000.")]
        public int StockQuantity { get; set; }
        
        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category must bebetween 2 and 50 characters.")]
        public string Category { get; set; } = null!;
        
        public bool IsActive { get; set; }
    }
}