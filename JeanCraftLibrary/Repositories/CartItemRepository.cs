using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly JeanCraftContext _context;

        public CartItemRepository(JeanCraftContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<CartItem> Createcart(CartItemRequest cart)
        {
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                ProductId = cart.ProductId,
                Quantity = cart.Quantity
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> Deletecart(Guid id)
        {
            var cart = await _context.CartItems.FindAsync(id);
            if (cart != null)
            {
                _context.CartItems.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CartItem>> GetAllcart()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> GetcartById(Guid id)
        {
            return await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CartItem> Updatecart(CartItem cart)
        {
            var existingEntity = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cart.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(cart);
            }
            else
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return cart;
        }
    }
}
