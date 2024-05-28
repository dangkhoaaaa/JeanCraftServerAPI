using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCardController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCart;

        public ShoppingCardController(IShoppingCartService shoppingCart) 
        {
            _shoppingCart = shoppingCart;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetAllCard()
        {
            var Cards = await _shoppingCart.GetAllShopingcart();
            return Ok(Cards); 
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetCardById(Guid id)
        {
            var Card = await _shoppingCart.GetShopingcartById(id);
            return Ok(Card);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateCard([FromBody] ShoppingCart card)
        {
            var createCard = await _shoppingCart.CreateShopingcart(card);
            return Ok(createCard);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateCard([FromBody] ShoppingCart cart)
        {
            var updateCard = await _shoppingCart.UpdateShopingcart(cart);
            return Ok(updateCard);
        }
    }
}
