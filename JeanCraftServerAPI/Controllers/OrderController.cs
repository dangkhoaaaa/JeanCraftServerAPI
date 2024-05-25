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

        // GET: api/MentorUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllMentors()
        {
            var mentors = await _orderService.GetAll();
            return Ok(mentors);
        }

        // GET: api/MentorUser/{id}
        [HttpGet("GetMentorById/{{id}}")]
        public async Task<ActionResult<Order>> GetMentorById(Guid id)
        {
            var mentor = _orderService.GetDetailOne(id);
            if (mentor == null)
            {
                return NotFound();
            }
            return Ok(mentor);
        }





        // POST: api/MentorUser
        [HttpPost]
        public async Task<ActionResult> AddMentor([FromBody] Order mentor)
        {
            if (mentor == null)
            {
                return BadRequest();
            }

            await _orderService.Add(mentor);
            return CreatedAtAction(nameof(GetMentorById), new { id = mentor.Id }, mentor);
        }

        // PUT: api/MentorUser/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMentor(Guid id, [FromBody] Order mentor)
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

            await _orderService.Update(mentor);
            return NoContent();
        }

        // DELETE: api/MentorUser/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMentor(Guid id)
        {
            var mentor = await _orderService.GetOne(id);
            if (mentor == null)
            {
                return NotFound();
            }

            await _orderService.Delete(id);
            return NoContent();
        }

    }
}
