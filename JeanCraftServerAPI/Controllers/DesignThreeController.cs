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
    public class DesignThreeController : ControllerBase
    {
        private readonly IDesignThreeService _designThreeService;
        private readonly IMapper _mapper;

        public DesignThreeController(IDesignThreeService designThreeService, IMapper mapper)
        {
            _designThreeService = designThreeService;
            _mapper = mapper;
        }

        [HttpGet("getDesignThreeList")]
        public async Task<IActionResult> GetAllDesignThrees()
        {
            var designThrees = await _designThreeService.GetAllDesignThrees();
            return Ok(designThrees);
        }

        [HttpGet("getDesignThreeById/{id}")]
        public async Task<IActionResult> GetDesignThreeById(Guid id)
        {
            var designThree = await _designThreeService.GetDesignThreeById(id);
            if (designThree == null)
            {
                return NotFound();
            }
            return Ok(designThree);
        }

        [HttpGet("findDesignThreeByComponents")]
        public async Task<IActionResult> FindDesignThreeByComponents([FromQuery] Guid? stitchingThreadColor, [FromQuery] Guid? buttonAndRivet)
        {
            var designThreeId = await _designThreeService.FindDesignThreeByComponentsAsync(stitchingThreadColor, buttonAndRivet);
            if (designThreeId == null)
            {
                return NotFound();
            }
            return Ok(designThreeId);
        }

        [HttpPost("createDesignThree")]
        public async Task<IActionResult> CreateDesignThree([FromBody] DesignThreeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var newdesignThree = await _designThreeService.CreateDesignThree(request);
                return CreatedAtAction(nameof(GetDesignThreeById), new { id = newdesignThree.DesignThreeId }, newdesignThree);
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
