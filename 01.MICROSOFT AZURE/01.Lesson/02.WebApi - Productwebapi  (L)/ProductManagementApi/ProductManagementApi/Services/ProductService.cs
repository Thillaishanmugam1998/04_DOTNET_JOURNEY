using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;

namespace ProductManagementApi.Services
{
    public class ProductService : IProductService
    {
        #region --- 01.PRIVATE VARIABLES ---
        // Repository instance used to perform database operations
        private readonly IProductRepository _repository;

        // Logger instance used to log service-level activities
        private readonly ILogger<ProductService> _logger;
        #endregion

        #region --- 02.CONSTRUCTOR ---
        // Constructor injection to receive the repository dependency
        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        #endregion

        #region ---03. GET ALL PRODUCTS---
        // Returns all products after converting them into response DTOs
        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Service call started for fetching all products.");

                // Get all product entities from the database through the repository
                var products = await _repository.GetAllAsync();

                // Create an empty list to store the converted DTO objects
                var productDtos = new List<ProductResponseDto>();

                // Loop through each product entity
                foreach (var product in products)
                {
                    // Convert the Product entity into ProductResponseDto
                    var dto = MapToResponseDto(product);

                    // Add the converted DTO to the final list
                    productDtos.Add(dto);
                }

                // Return the list of ProductResponseDto objects
                return productDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error occurred while fetching all products.");
                throw;
            }
        }
        #endregion

        #region ---04. GET PRODUCT BY ID---
        // Returns a single product by Id after converting it into response DTO
        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service call started for fetching product with Id {ProductId}.", id);

                var product = await _repository.GetByIdAsync(id);

                // Return null if product is not found
                if (product == null)
                    return null;

                return MapToResponseDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error occurred while fetching product with Id {ProductId}.", id);
                throw;
            }
        }
        #endregion

        #region ---05. CREATE PRODUCT---
        // Creates a new Product entity from the create DTO and saves it to the database
        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Service call started for creating a new product named {ProductName}.", dto.Name);

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    Category = dto.Category,
                    IsActive = true, // New product is active by default
                    CreatedOn = DateTime.UtcNow // Store the product creation time in UTC
                };

                // Add the new product to the database
                await _repository.AddAsync(product);

                // Persist the changes
                await _repository.SaveAsync();

                // Return the created product as response DTO
                return MapToResponseDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error occurred while creating product named {ProductName}.", dto.Name);
                throw;
            }
        }
        #endregion
        
        #region ---06. UPDATE PRODUCT---
        // Updates an existing product if found
        public async Task<bool> UpdateProductAsync(ProductUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Service call started for updating product with Id {ProductId}.", dto.Id);

                var existingProduct = await _repository.GetByIdAsync(dto.Id);

                // Return false if the product does not exist
                if (existingProduct == null)
                    return false;

                // Update product properties with new values
                existingProduct.Name = dto.Name;
                existingProduct.Description = dto.Description;
                existingProduct.Price = dto.Price;
                existingProduct.StockQuantity = dto.StockQuantity;
                existingProduct.Category = dto.Category;
                existingProduct.IsActive = dto.IsActive;

                // Mark the entity as updated
                _repository.Update(existingProduct);

                // Save changes to the database
                await _repository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error occurred while updating product with Id {ProductId}.", dto.Id);
                throw;
            }
        }
        #endregion

        #region ---07. DELETE PRODUCT---
        // Deletes an existing product if found
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service call started for deleting product with Id {ProductId}.", id);

                var existingProduct = await _repository.GetByIdAsync(id);

                // Return false if the product does not exist
                if (existingProduct == null)
                    return false;

                // Mark the entity for deletion
                _repository.Delete(existingProduct);

                // Save changes to the database
                await _repository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error occurred while deleting product with Id {ProductId}.", id);
                throw;
            }
        }
        #endregion

        #region ---08. PRIVATE HELPER METHODS---
        // Converts Product entity into ProductResponseDto
        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                IsActive = product.IsActive,
                CreatedOn = product.CreatedOn
            };
        }
        #endregion
    }
}