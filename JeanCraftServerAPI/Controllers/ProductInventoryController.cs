using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryController : ControllerBase
    {
        private readonly IProductInventoryService _productInventoryService;

        public ProductInventoryController(IProductInventoryService productInventoryService) 
        {
            _productInventoryService = productInventoryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var productInventorys = await _productInventoryService.GetProductInventorys();
            return Ok(ResponseHandle<IEnumerable<ProductInventory>>.Success(productInventorys));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var productInventory = await _productInventoryService.GetProductInventoryByID(id);
            return Ok(ResponseHandle<ProductInventory>.Success(productInventory));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productInventory = await _productInventoryService.GetProductInventoryByID(id);
            if(productInventory == null)
            {
                return Ok(ResponseHandle<ProductInventory>.Error("Invalid ProductInventory"));
            }
             await _productInventoryService.DeleteProductInventory(id);
            
            return Ok(ResponseHandle<ProductInventory>.Success(productInventory));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProductInventory([FromBody] ProductInventoryRequest productInventoryRequest)
        {
            var product = await _productInventoryService.UpdateProductInventory(productInventoryRequest);
            if (product == null)
            {
                return Ok(ResponseHandle<ProductInventory>.Error("Update failse"));
            }
            return Ok(ResponseHandle<ProductInventory>.Success(product));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProductInventory([FromBody] ProductInventoryRequest productRequest)
        {
            
            var product = await _productInventoryService.CreateProductInventory(productRequest);
            if (product == null)
            {
                return Ok(ResponseHandle<ProductInventory>.Error("Create product failse"));
            }
            return Ok(ResponseHandle<ProductInventory>.Success(product));
        }
    }
}
