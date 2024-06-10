using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Response;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCart;
        private readonly IMapper _mapper;
        public ShoppingCartController(IShoppingCartService shoppingCart, IMapper mapper) 
        {
            _shoppingCart = shoppingCart;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetAllCart()
        {
            var carts = await _shoppingCart.GetAllShopingcart();
            return Ok(carts); 
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetCartById(Guid id)
        {
            var cart = await _shoppingCart.GetShopingcartById(id);
            return Ok(cart);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateCart([FromBody] ShoppingCart cart)
        {
            var createCart = await _shoppingCart.CreateShopingcart(cart);
            return Ok(createCart);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateCart([FromBody] ShoppingCart cart)
        {
            var updateCart = await _shoppingCart.UpdateShopingcart(cart);
            return Ok(updateCart);
        }

        [HttpGet("GetCartForUser")]
        public async Task<ActionResult> GetCartForUser([FromQuery] Guid userId)
        {
            var carts = await _shoppingCart.GetCartForUser(userId);
            IEnumerable<ShoppingCartItemResponse> response = _mapper.Map<IEnumerable<ShoppingCartItemResponse>>(carts);
            foreach (var item in response)
            {
                item.Cart.Size = item.Cart.Product.Size;
            }
            return Ok(response);
        }
    }
}
