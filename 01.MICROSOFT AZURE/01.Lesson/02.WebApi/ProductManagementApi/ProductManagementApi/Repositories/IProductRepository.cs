using ProductManagementApi.Models;
namespace ProductManagementApi.Repositories
{
    public interface IProductRepository
    {
        // Returns all products from the database
        public Task<List<Product>> GetAllAsync();

        // Returns a single product based on the given Id
        public Task<Product?> GetByIdAsync(int id);

        // Adds a new product to the DbContext
        public Task AddAsync(Product product);

        // Marks an existing product as modified
        public void Update(Product product);

        // Marks a product for deletion
        public void Delete(Product product);

        // Saves all pending changes to the database
        public Task SaveAsync();
    }
}