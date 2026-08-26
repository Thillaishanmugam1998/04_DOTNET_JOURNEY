using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{
    // Defines the base route for this controller as: api/products
    [Route("api/[controller]")]

    // Enables automatic model validation and API-specific behaviors
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Service instance used to handle product-related business logic
        private readonly IProductService _service;

        // Logger instance used to log controller-level activities
        private readonly ILogger<ProductsController> _logger;

        // Constructor injection to receive the product service dependency
        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Handles HTTP GET request to return all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("HTTP GET request received for fetching all products.");

                // Call the service layer to fetch all products
                var products = await _service.GetAllProductsAsync();

                // Return 200 OK response with the product list
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET all products request.");
                return StatusCode(500, "An error occurred while fetching products.");
            }
        }

        // Handles HTTP GET request to return a single product by Id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation("HTTP GET request received for fetching product with Id {ProductId}.", id);

                // Call the service layer to fetch the product by Id
                var product = await _service.GetProductByIdAsync(id);

                // Return 404 Not Found if the product does not exist
                if (product == null)
                    return NotFound($"Product with Id {id} not found.");

                // Return 200 OK response with the product data
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET product by Id request for Id {ProductId}.", id);
                return StatusCode(500, $"An error occurred while fetching product with Id {id}.");
            }
        }

        // Handles HTTP POST request to create a new product
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductCreateDto dto)
        {
            try
            {
                _logger.LogInformation("HTTP POST request received for creating a product named {ProductName}.", dto.Name);

                // Call the service layer to create the product
                var createdProduct = await _service.CreateProductAsync(dto);

                // Return 201 Created response with location header and created product data
                return CreatedAtAction(
                    nameof(GetProductById),
                    new { id = createdProduct.Id },
                    createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing CREATE product request for product {ProductName}.", dto.Name);
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        // Handles HTTP PUT request to update an existing product by Id
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("HTTP PUT request received for updating product with Id {ProductId}.", id);

                if (id != dto.Id)
                {
                    _logger.LogInformation("Id Missmatch, Id: {ProductId} and Dto.Id {Dto.Id}", id, dto.Id);
                    return BadRequest("Id Missmatch");

                }

                // Call the service layer to update the product
                var updated = await _service.UpdateProductAsync(dto);

                // Return 404 Not Found if the product does not exist
                if (!updated)
                    return NotFound($"Product with Id {id} not found.");

                // Return 204 No Content when update is successful
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing UPDATE product request for Id {ProductId}.", id);
                return StatusCode(500, $"An error occurred while updating product with Id {id}.");
            }
        }

        // Handles HTTP DELETE request to remove an existing product by Id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("HTTP DELETE request received for deleting product with Id {ProductId}.", id);

                // Call the service layer to delete the product
                var deleted = await _service.DeleteProductAsync(id);

                // Return 404 Not Found if the product does not exist
                if (!deleted)
                    return NotFound($"Product with Id {id} not found.");

                // Return 204 No Content when deletion is successful
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing DELETE product request for Id {ProductId}.", id);
                return StatusCode(500, $"An error occurred while deleting product with Id {id}.");
            }
        }
    }
}