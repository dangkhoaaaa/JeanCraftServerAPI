using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentController(IComponentService componentService) 
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Component>>> GetAllComponent() 
        {
           var Components = await _componentService.GetAllComponent();
            return Ok(Components);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Component>> GetComponentById(Guid id)
        {
            var component = await _componentService.GetComponentById(id);
            return Ok(component);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAComponentType([FromBody] ComponentDTO component)
        {
            var createAComponent = await _componentService.CreateComponent(component);
            return Ok(createAComponent);
        }

        

        [HttpPut("id")]
        public async Task<IActionResult> UpdateComponent([FromQuery]Guid id, [FromBody]ComponentDTO component)
        {
            var updateComponent = await _componentService.UpdateComponent(id, component);
            if(updateComponent == null)
            {
                return NotFound();
            }
            return Ok(updateComponent);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteComponent(Guid id)
        {
            var result = await _componentService.DeleteComponent(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
