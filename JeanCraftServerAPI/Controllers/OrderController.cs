using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
  
    [ApiController]
    [Route("api/Order")]
    public class OrderController : ControllerBase
    {
       
        private readonly IOrderService _orderService;

        public OrderController(IOrderService OrderServicece)
        {
            _orderService = OrderServicece;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrder()
        {
            var mentors = await _orderService.GetAll();
            return Ok(mentors);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = _orderService.GetDetailOne(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        [HttpPost("AddOrder")]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            await _orderService.Add(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        
        [HttpPut("UpdateOrder/{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] Order order)
        {
            //if (mentor == null || id != mentor.Id)
            //{
            //    return BadRequest();
            //}

            //var existingMentor = await _mentorUserService.GetOne(id);
            //if (existingMentor == null)
            //{
            //    return NotFound();
            //}

            await _orderService.Update(order);
            return NoContent();
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOne(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.Delete(id);
            return NoContent();
        }

    }
}
