using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IProductService
    {
        Task<Product?> CreateProduct(Product product);
        Task<Product?> DeleteProduct(Product product);
        Task<IList<Product>> GetProductList();
        Task<Product?> GetProductByID(Guid id);
        Task<Product[]?> GetProductByName(string name);
        Task<Product?> UpdateProduct(Product product);
    }
}
