using JeanCraftLibrary;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;

namespace JeanCraftServerAPI.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartItemService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CartItem> Createcart(CartItemRequest cart)
        {
            return await _unitOfWork.CartItemRepository.Createcart(cart);
        }

        public async Task<bool> Deletecart(Guid id)
        {
            var deleteCart = await _unitOfWork.CartItemRepository.GetcartById(id);
            if (deleteCart == null)
            {
                return false;
            }
            return await _unitOfWork.CartItemRepository.Deletecart(id);
        }

        public async Task<IEnumerable<CartItem>> GetAllcart()
        {
            return await _unitOfWork.CartItemRepository.GetAllcart();
        }

        public async Task<CartItem> GetcartById(Guid id)
        {
            return await _unitOfWork.CartItemRepository.GetcartById(id);
        }

        public async Task<CartItem> Updatecart(Guid id, CartItemRequest cart)
        {
            var cartUpdate = await _unitOfWork.CartItemRepository.GetcartById(id);
            if(cartUpdate == null)
            {
                return null;
            }
            if(cart.Quantity.HasValue)
            {
                cartUpdate.Quantity = cart.Quantity.Value;
            }
            if (cart.ProductId.HasValue)
            {
                cartUpdate.ProductId = cart.ProductId.Value;
            }
            await _unitOfWork.CartItemRepository.Updatecart(cartUpdate);
            return cartUpdate;
        }
    }
}
