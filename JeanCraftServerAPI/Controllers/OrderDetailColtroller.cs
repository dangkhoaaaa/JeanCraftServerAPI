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

        // GET: api/MentorUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllMentors()
        {
            var mentors = await _orderDetailService.GetAll();
            return Ok(mentors);
        }

        // GET: api/MentorUser/{id}
        [HttpGet("GetMentorById/{{id}}")]
        public async Task<ActionResult<OrderDetail>> GetMentorById(Guid id)
        {
            var mentor = _orderDetailService.GetDetailOne(id);
            if (mentor == null)
            {
                return NotFound();
            }
            return Ok(mentor);
        }





        // POST: api/MentorUser
        [HttpPost]
        public async Task<ActionResult> AddMentor([FromBody] OrderDetail mentor)
        {
            if (mentor == null)
            {
                return BadRequest();
            }

            await _orderDetailService.Add(mentor);
            return CreatedAtAction(nameof(GetMentorById), new { id = mentor.ProductId }, mentor);
        }

        // PUT: api/MentorUser/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMentor(Guid id, [FromBody] OrderDetail mentor)
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

            await _orderDetailService.Update(mentor);
            return NoContent();
        }

        // DELETE: api/MentorUser/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMentor(Guid id)
        {
            var mentor = await _orderDetailService.GetOne(id);
            if (mentor == null)
            {
                return NotFound();
            }

            await _orderDetailService.Delete(id);
            return NoContent();
        }

    }
}
