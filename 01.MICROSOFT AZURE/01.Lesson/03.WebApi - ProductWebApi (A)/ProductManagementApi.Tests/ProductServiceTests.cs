using Microsoft.Extensions.Logging;
using Moq;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
using ProductManagementApi.Services;

namespace ProductManagementApi.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsAsync_Returns_Mapped_ProductResponseDtos()
        {
            // Arrange
            // Create sample product data that will act as the fake data
            // returned by the repository during this test.
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Keyboard",
                    Description = "Mechanical keyboard",
                    Price = 2500,
                    StockQuantity = 10,
                    Category = "Electronics",
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow
                }
            };

            // Create a mock repository so we can control what data
            // is returned without depending on a real database.
            var repositoryMock = new Mock<IProductRepository>();

            // Configure the mocked repository to return our sample product list
            // when GetAllAsync is called by the service.
            repositoryMock.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(products);

            // Create a mock logger because ProductService expects an ILogger<ProductService>
            // in its constructor, but we do not need real logging behavior in this test.
            var loggerMock = new Mock<ILogger<ProductService>>();

            // Create the ProductService instance by injecting the mocked repository
            // and mocked logger.
            var service = new ProductService(repositoryMock.Object, loggerMock.Object);

            // Act
            // Call the service method that we want to test.
            var result = await service.GetAllProductsAsync();

            // Assert
            // Verify that exactly one product response DTO is returned.
            Assert.Single(result);

            // Verify that the returned product data is mapped correctly
            Assert.Equal("Keyboard", result[0].Name);
            Assert.Equal("Electronics", result[0].Category);
            Assert.Equal(2500, result[0].Price);
        }
    }
}