using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services;
using JeanCraftServerAPI.Services.Interface;
using JeanCraftServerAPI.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<ActionResult<PageResult<OrderFormModel>>> GetAllOrders([FromQuery] FormSearch search)
        {
            var ordersPagedResult = _orderService.GetAllPaging(search.currentPage, search.pageSize);

            if (ordersPagedResult == null || ordersPagedResult.Items.Count == 0)
            {
                return NotFound("No orders found.");
            }

            return Ok(ordersPagedResult);
        }

        [HttpGet("GetTotalCount")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            var totalCount = _orderService.GetTotalCount();
            return Ok(totalCount);
        }

        [HttpGet("GetAllSucessOrders")]
        public async Task<ActionResult<IEnumerable<OrderFormModel>>> GetAllSucessOrders([FromQuery] FormSearch search)
        {
            var orders = _orderService.GetAllPagingSucessfully(search.currentPage, search.pageSize);
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

        [HttpGet("GetOrdersByDate")]
        public async Task<ActionResult<IEnumerable<OrderFormModel>>> GetOrdersByDate([FromQuery] string date)
        {
            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                var orders = await _orderService.GetOrdersByDateAsync(parsedDate);
                if (orders == null || orders.Count == 0)
                {
                    return NotFound("No orders found for the specified date.");
                }
                return Ok(orders);
            }
            else
            {
                return BadRequest("Invalid date format. Please use 'dd-MM-yyyy'.");
            }
        }

        [HttpGet("GetOrderCountByDate")]
        public async Task<IActionResult> GetOrderCountByDate([FromQuery] string date)
        {
            Console.WriteLine($"Received date string: {date}");

            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                var orderCount = await _orderService.GetOrderCountByDateAsync(parsedDate);

                return Ok(new { Date = parsedDate.ToString("dd-MM-yyyy"), OrderCount = orderCount });
            }
            else
            {
                return BadRequest("Invalid date format. Please use 'dd-MM-yyyy'.");
            }
        }


        [HttpPost("AddOrder")]
        public async Task<ActionResult<OrderCreateRequestModel>> AddOrder([FromBody] OrderCreateRequestModel order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            try
            {
                Guid id = await _orderService.Add(order);
                return Ok(ResponseHandle<Guid>.Success(id));
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

        [HttpPut("UpdateStatusOrder")]
        public async Task<ActionResult<OrderUpdateRequestModel>> UpdateStatusOrder(Guid id, string status)
        {
            var existingOrder = await _orderService.GetOne(id);
            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            try
            {
                await _orderService.UpdateStatus(id, status);
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
