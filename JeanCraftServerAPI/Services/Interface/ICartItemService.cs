using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Repositories.Interface;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface ICartItemService 
    {
        Task<IEnumerable<CartItem>> GetAllcart();

        Task<CartItem> GetcartById(Guid id);
        Task<CartItem> Createcart(CartItemRequest cart);
        Task<CartItem> Updatecart(Guid id, CartItemRequest cart);
        Task<bool> Deletecart(Guid id);
    }
}
