using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignOneController : ControllerBase
    {
        private readonly IDesignOneService _designOneService;
        private readonly IMapper _mapper;

        public DesignOneController(IDesignOneService designOneService, IMapper mapper)
        {
            _designOneService = designOneService;
            _mapper = mapper;
        }

        [HttpGet("getDesignOneList")]
        public async Task<IActionResult> GetAllDesignOnes()
        {
            var designOnes = await _designOneService.GetAllDesignOnes();
            return Ok(designOnes);
        }

        [HttpGet("getDesignOneById/{id}")]
        public async Task<IActionResult> GetDesignOneById(Guid id)
        {
            var designOne = await _designOneService.GetDesignOneById(id);
            if (designOne == null)
            {
                return NotFound();
            }
            return Ok(designOne);
        }

        [HttpGet("findDesignOneByComponents")]
        public async Task<IActionResult> FindDesignOneByComponents([FromQuery] Guid? fit, [FromQuery] Guid? length, [FromQuery] Guid? cuffs, [FromQuery] Guid? fly, [FromQuery] Guid? frontPocket, [FromQuery] Guid? backPocket)
        {
            var designOneId = await _designOneService.FindDesignOneByComponentsAsync(fit, length, cuffs, fly, frontPocket, backPocket);
            if (designOneId == null)
            {
                return NotFound();
            }
            return Ok(designOneId);
        }

        [HttpPost("createDesignOne")]
        public async Task<IActionResult> CreateDesignOne([FromBody] DesignOneRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                var designOneResponse = await _designOneService.CreateAsync(request);
                return CreatedAtAction(nameof(GetDesignOneById), new { id = designOneResponse.DesignOneId }, designOneResponse);
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
