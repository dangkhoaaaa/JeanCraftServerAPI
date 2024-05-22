using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> CreateProduct(Product product);
        Task<Product?> DeleteProduct(Product product);
        Task<Product[]?> GetProductList();
        Task<Product?> GetProductByID(Guid id);
        Task<Product[]?> GetProductByName(string name);
        Task<Product?> UpdateProduct(Product product);
    }
}
