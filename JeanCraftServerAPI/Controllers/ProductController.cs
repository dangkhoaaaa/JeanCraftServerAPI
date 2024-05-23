using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
        }

        [HttpGet("product/getProductList")]
        public async Task<IActionResult> GetProductList()
        {
            Product[] products = await _productService.GetProductList();
            return Ok(ResponseArrayHandle<Product>.Success(products));
        }

        [HttpGet("product/getProductByID/{productId}")]
        public async Task<IActionResult> GetProductByID(Guid productId)
        {
            Product product = await _productService.GetProductByID(productId);
            return Ok(ResponseHandle<Product>.Success(product));
        }
        [HttpDelete("product/deleteProduct/{productId}")]
        public async Task<IActionResult> DeteleProduct(Guid productId)
        {
            Product product = await _productService.GetProductByID(productId);
            if (product == null)
            {
                return Ok(ResponseHandle<LoginResponse>.Error("Invalid product"));
            }
            product = await _productService.DeleteProduct(product);
            return Ok(ResponseHandle<Product>.Success(product));
        }
    }
}
