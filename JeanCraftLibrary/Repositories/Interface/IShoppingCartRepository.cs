using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        Task<IEnumerable<ShoppingCart>> GetAllShopingcart();

        Task<ShoppingCart> GetShopingcartById(Guid id);
        Task<ShoppingCart> CreateShopingcart(ShoppingCart cart);
        Task<ShoppingCart> UpdateShopingcart(ShoppingCart cart);
        Task<bool> DeleteShopingcart(Guid id);
        Task<IEnumerable<ShoppingCart>> GetCartForUser(Guid userId);
    }
}
