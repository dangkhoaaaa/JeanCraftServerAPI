using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IProductService
    {
        Task<Product?> CreateProduct(Product product);
        Task<Product?> DeleteProduct(Product product);
        Task<Product[]?> GetProductList();
        Task<Product?> GetProductByID(Guid id);
        Task<Product[]?> GetProductByName(string name);
        Task<Product?> UpdateProduct(Product product);
        Task<Product?> CreateProductByBooking(Product product);
        Task<Product?> UpdateProductByBooking(Product product);

        Task<Product[]?> SearchProduct(SearchProductRequest filter);
    }
}
