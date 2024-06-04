using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IProductInventoryRepository : IGenericRepository<ProductInventory>
    {
        Task<ProductInventory> CreateProductInventory(ProductInventory product);
        Task<bool> DeleteProductInventory(Guid id);
        Task<IEnumerable<ProductInventory>> GetProductInventorys();
        Task<ProductInventory> GetProductInventoryByID(Guid id);
        Task<ProductInventory> UpdateProductInventory(ProductInventory product);
    }
}
