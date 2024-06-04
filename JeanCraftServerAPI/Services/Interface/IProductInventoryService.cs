using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IProductInventoryService
    {
        
        Task<ProductInventory> CreateProductInventory(ProductInventoryRequest productInventoryRequest);
        Task<bool> DeleteProductInventory(Guid id);
        Task<IEnumerable<ProductInventory>> GetProductInventorys();
        Task<ProductInventory> GetProductInventoryByID(Guid id);
        Task<ProductInventory> UpdateProductInventory(ProductInventoryRequest productInventoryRequest);

    }
}
