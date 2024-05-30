using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeanCraftServerAPI.Controllers
{
    [ApiController]
    [Route("api/Order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<OrderFormModel>>> GetAllOrders([FromQuery] FormSearch search)
        {
            var orders = _orderService.GetAllPaging(search.currentPage, search.pageSize);
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<OrderFormModel>> GetOrderById(Guid id, [FromQuery] FormSearch search)
        {
            var order = _orderService.GetDetailOne(id, search.currentPage, search.pageSize);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            return Ok(order);
        }

        [HttpPost("AddOrder")]
        public async Task<ActionResult<OrderFormModel>> AddOrder([FromBody] OrderCreateRequestModel order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            try
            {
                await _orderService.Add(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.AddressId }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateOrder")]
        public async Task<ActionResult<OrderUpdateRequestModel>> UpdateOrder(Guid id, [FromBody] OrderUpdateRequestModel order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            if (id != order.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingOrder = await _orderService.GetOne(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            try
            {
                await _orderService.Update(order);
                var updatedOrder = await _orderService.GetOne(id); // Fetch the updated entity
                return CreatedAtAction(nameof(GetOrderById), new { id = updatedOrder.Id }, updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOne(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            try
            {
                await _orderService.Delete(id);
                return Ok($"Order with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
