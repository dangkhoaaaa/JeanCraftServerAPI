﻿using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService) 
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var cart = await _cartItemService.GetAllcart();
            return Ok(cart);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetCartById(Guid id)
        {
            var cart = await _cartItemService.GetcartById(id);
            return Ok(cart);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCart(CartItemRequest cartItemRequest)
        {
            var cart = await _cartItemService.Createcart(cartItemRequest);
            return Ok(cart);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart([FromQuery] Guid id, [FromBody]  CartItemRequest cartItemRequest)
        {
            var cart = await _cartItemService.Updatecart(id, cartItemRequest);
            if(cart != null)
            {
                return Ok(cart);
            }
            return BadRequest();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var result = await _cartItemService.Deletecart(id);
            if(!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}