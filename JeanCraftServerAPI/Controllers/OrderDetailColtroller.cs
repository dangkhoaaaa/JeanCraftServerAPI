using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{

    [ApiController]
    [Route("api/OrderDetail")]
    public class OrderDetailColtroller : ControllerBase
    {

        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailColtroller(IOrderDetailService OrderDetailRepository)
        {
            _orderDetailService = OrderDetailRepository;
        }

        [HttpGet("GetAllOrderDetail")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetail()
        {
            var orderdetail = await _orderDetailService.GetAll();
            return Ok(orderdetail);
        }

        [HttpGet("GetMentorById/{{id}}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetailById(Guid id)
        {
            var orderdetail = _orderDetailService.GetDetailOne(id);
            if (orderdetail == null)
            {
                return NotFound();
            }
            return Ok(orderdetail);
        }

        
        [HttpPost("CreateOrderDetail")]
        public async Task<ActionResult> AddOrderDetail([FromBody] OrderDetail orderdetail)
        {
            if (orderdetail == null)
            {
                return BadRequest();
            }

            await _orderDetailService.Add(orderdetail);
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderdetail.ProductId }, orderdetail);
        }

       
        [HttpPut("UpdateOrderDetail/{id}")]
        public async Task<ActionResult> UpdateOrderDetail(Guid id, [FromBody] OrderDetail orderdetail)
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

            await _orderDetailService.Update(orderdetail);
            return NoContent();
        }

        [HttpDelete("DeleteOrderDetail/{id}")]
        public async Task<ActionResult> DeleteOrderDetail(Guid id)
        {
            var orderdetail = await _orderDetailService.GetOne(id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            await _orderDetailService.Delete(id);
            return NoContent();
        }

    }
}
