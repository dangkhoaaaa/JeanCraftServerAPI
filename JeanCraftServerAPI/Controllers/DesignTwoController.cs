using AutoMapper;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignTwoController : ControllerBase
    {
        private readonly IDesignTwoService _designTwoService;
        private readonly IMapper _mapper;

        public DesignTwoController(IDesignTwoService designTwoService, IMapper mapper)
        {
            _designTwoService = designTwoService;
            _mapper = mapper;
        }

        [HttpGet("getDesignTwoList")]
        public async Task<IActionResult> GetAllDesignTwos()
        {
            var designTwos = await _designTwoService.GetAllDesignTwos();
            return Ok(designTwos);
        }

        [HttpGet("getDesignTwoById/{id}")]
        public async Task<IActionResult> GetDesignTwoById(Guid id)
        {
            var designTwo = await _designTwoService.GetDesignTwoById(id);
            if (designTwo == null)
            {
                return NotFound();
            }
            return Ok(designTwo);
        }

        [HttpGet("findDesignTwoByComponents")]
        public async Task<IActionResult> FindDesignTwoByComponents([FromQuery] Guid? finishing, [FromQuery] Guid? fabricColor)
        {
            var designTwoId = await _designTwoService.FindDesignTwoByComponentsAsync(finishing, fabricColor);
            if (designTwoId == null)
            {
                return NotFound();
            }
            return Ok(designTwoId);
        }

        [HttpPost("createDesignTwo")]
        public async Task<IActionResult> CreateDesignTwo([FromBody] DesignTwoRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var newdesignTwo = await _designTwoService.CreateDesignTwo(request);
                return CreatedAtAction(nameof(GetDesignTwoById), new { id = newdesignTwo.DesignTwoId }, newdesignTwo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the design");
            }
        }
    }
}
