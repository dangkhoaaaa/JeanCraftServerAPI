using JeanCraftLibrary.Entity;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCart>> GetAllShopingcart();

        Task<ShoppingCart> GetShopingcartById(Guid id);
        Task<ShoppingCart> CreateShopingcart(ShoppingCart cart);
        Task<ShoppingCart> UpdateShopingcart(ShoppingCart cart);
        Task<bool> DeleteShopingcart(Guid id);
        Task<IEnumerable<ShoppingCart>> GetCartForUser(Guid userId);
    }
}

