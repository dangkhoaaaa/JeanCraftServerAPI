using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ProductInventoryRepository : GenericRepository<ProductInventory>, IProductInventoryRepository
    {
        private readonly JeanCraftContext _dbContext;

        public ProductInventoryRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductInventory?> CreateProductInventory(ProductInventory product)
        {
            _dbContext.ProductInventories.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductInventory(Guid id)
        {
            var productInventory = await _dbContext.Products.FindAsync(id);
            if(productInventory == null) 
            {
                return false;
            }
            _dbContext.Products.Remove(productInventory);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductInventory> GetProductInventoryByID(Guid id)
        {
            return await _dbContext.ProductInventories.AsNoTracking().FirstOrDefaultAsync(p => p.ProductInventoryId == id);
        }

        public async Task<IEnumerable<ProductInventory>> GetProductInventorys()
        {
            return await _dbContext.ProductInventories.ToListAsync();
        }

        public async Task<ProductInventory> UpdateProductInventory(ProductInventory product)
        {
            var existingEntity = await _dbContext.ProductInventories.FirstOrDefaultAsync(p => p.ProductInventoryId == product.ProductInventoryId);
            if (existingEntity == null)
            {
                return null;
            }
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
    
}
