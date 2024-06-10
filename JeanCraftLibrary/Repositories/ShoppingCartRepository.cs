using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly JeanCraftContext _dbContext;

        public ShoppingCartRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShoppingCart> CreateShopingcart(ShoppingCart cart)
        {
            //var cartToAdd = new ShoppingCart
            //{
            //    CartId = cart.CartId,
            //    UserId = cart.UserId
            //};
            _dbContext.ShoppingCarts.Add(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteShopingcart(Guid id)
        {
            var cart = await _dbContext.ShoppingCarts.AsNoTracking().FirstOrDefaultAsync(c => c.CartId == id);
            if (cart != null)
            {
                _dbContext.ShoppingCarts.Remove(cart);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllShopingcart()
        {
            return await _dbContext.ShoppingCarts.ToListAsync();
        }

        public async Task<IEnumerable<ShoppingCart>> GetCartForUser(Guid userId)
        {
            return await _dbContext.ShoppingCarts.AsNoTracking().Where(x => x.UserId == userId).Include(x => x.Cart).ThenInclude(x => x.Product).ToListAsync();
        }

        public async Task<ShoppingCart> GetShopingcartById(Guid id)
        {
            return await _dbContext.ShoppingCarts.AsNoTracking().FirstOrDefaultAsync(c => c.CartId == id);
        }

        public async Task<ShoppingCart> UpdateShopingcart(ShoppingCart cart)
        {
            var shoppincart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(c => c.CartId == cart.CartId);
            if (shoppincart != null)
            {
                _dbContext.Entry(shoppincart).CurrentValues.SetValues(cart);
            }
            else
            {
                return null;
            }
            await _dbContext.SaveChangesAsync();
            return cart;
        }
    }
}
