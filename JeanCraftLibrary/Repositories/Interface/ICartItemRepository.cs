using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetAllcart();

        Task<CartItem> GetcartById(Guid id);
        Task<CartItem> Createcart(CartItemRequest cart);
        Task<CartItem> Updatecart(CartItem cart);
        Task<bool> Deletecart(Guid id);
    }
}
