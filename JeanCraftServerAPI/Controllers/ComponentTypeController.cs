using JeanCraftLibrary.Entity;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentTypeController : ControllerBase
    {
        private readonly IComponentTypeService _componentTypeService;

        public ComponentTypeController(IComponentTypeService componentTypeService)
        {
            _componentTypeService = componentTypeService;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ComponentType>>> GetAllComponentType(string? search, int currentPage, int pageSize)
        {
            var ComponentTypes = await _componentTypeService.GetAllComponent(search, currentPage, pageSize);
            return Ok(ComponentTypes);
        }

        [HttpGet("typeID/{id}")]
        public async Task<ActionResult<IEnumerable<ComponentType>>> GetComponentById(Guid id)
        {
            var componentType = await _componentTypeService.GetComponentById(id);
            return Ok(componentType);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAComponentType([FromBody] ComponentType componentType)
        {
            var createAComponentType = await _componentTypeService.CreateComponent(componentType);
            return Ok(createAComponentType);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateComponentType([FromBody] ComponentType componentType)
        {
            var updateComponentType = await _componentTypeService.UpdateComponent(componentType);
            return Ok(updateComponentType);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAComponentType(Guid id)
        {
            var result = await _componentTypeService.DeleteComponent(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
    }
