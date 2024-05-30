using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using JeanCraftLibrary.Model.Request;

namespace JeanCraftServerAPI.Controllers
{
    [ApiController]
    [Route("api/OrderDetail")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("GetAllOrderDetails")]
        public async Task<ActionResult<IEnumerable<OrderDetailFormModel>>> GetAllOrderDetails([FromQuery] FormSearch search)
        {
            var orderDetails =  _orderDetailService.GetAllPaging(search.currentPage, search.pageSize);
            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound("No order details found.");
            }
            return Ok(orderDetails);
        }

        [HttpGet("GetOrderDetailById/{id}")]
        public async Task<ActionResult<OrderDetailFormModel>> GetOrderDetailById(Guid id, [FromQuery] FormSearch search)
        {
            var orderDetail =  _orderDetailService.GetDetailOne(id, search.currentPage, search.pageSize);
            if (orderDetail == null)
            {
                return NotFound($"Order detail with ID {id} not found.");
            }
            return Ok(orderDetail);
        }

        [HttpPost("CreateOrderDetail")]
        public async Task<ActionResult<OrderDetailCreateRequestModel>> AddOrderDetail([FromBody] OrderDetailCreateRequestModel orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Order detail is null.");
            }

            try
            {
                await _orderDetailService.Add(orderDetail);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetail.OrderId }, orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateOrderDetail")]
        public async Task<ActionResult<OrderDetailUpdateRequestModel>> UpdateOrderDetail(Guid id, [FromBody] OrderDetailUpdateRequestModel orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Order detail is null.");
            }

            if (id != orderDetail.OrderId)
            {
                return BadRequest("ID mismatch.");
            }

            var existingOrderDetail = await _orderDetailService.GetOne(id);
            if (existingOrderDetail == null)
            {
                return NotFound($"Order detail with ID {id} not found.");
            }

            try
            {
                await _orderDetailService.Update(orderDetail);
                var updatedOrderDetail = await _orderDetailService.GetOne(id);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = updatedOrderDetail.OrderId }, updatedOrderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteOrderDetail/{id}")]
        public async Task<ActionResult> DeleteOrderDetail(Guid id)
        {
            var orderDetail = await _orderDetailService.GetOne(id);
            if (orderDetail == null)
            {
                return NotFound($"Order detail with ID {id} not found.");
            }

            try
            {
                await _orderDetailService.Delete(id);
                return Ok($"Order detail with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
