using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ShoppingCart> CreateShopingcart(ShoppingCart cart)
        {
            return await _unitOfWork.ShoppingCartRepository.CreateShopingcart(cart);
        }

        public async Task<bool> DeleteShopingcart(Guid id)
        {
            var deleteCart = await _unitOfWork.ShoppingCartRepository.GetShopingcartById(id);
            if (deleteCart != null)
            {
                return await _unitOfWork.ShoppingCartRepository.DeleteShopingcart(id);
            }
            return false;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllShopingcart()
        {
            return await _unitOfWork.ShoppingCartRepository.GetAllShopingcart();
        }

        public async Task<ShoppingCart> GetShopingcartById(Guid id)
        {
            return await _unitOfWork.ShoppingCartRepository.GetShopingcartById(id);
        }

        public async Task<ShoppingCart> UpdateShopingcart(ShoppingCart cart)
        {
            return await _unitOfWork.ShoppingCartRepository.UpdateShopingcart(cart);
        }
    }
}
