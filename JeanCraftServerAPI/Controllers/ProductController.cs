using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JeanCraftLibrary.Model.Request;
using AutoMapper;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IConfiguration configuration, IMapper mapper)
        {
            _productService = productService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("product/getProductList")]
        public async Task<IActionResult> GetProductList()
        {
            var products = await _productService.GetProductList();
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
                return Ok(ResponseHandle<Product>.Error("Invalid product"));
            }
            product = await _productService.DeleteProduct(product);
            return Ok(ResponseHandle<Product>.Success(product));
        }
        [HttpPost("product/createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            
            product = await _productService.CreateProduct(product);
            if (product == null)
            {
                return Ok(ResponseHandle<Product>.Error("Create product failse"));
            }
            return Ok(ResponseHandle<Product>.Success(product));
        }
        [HttpPut("product/updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            product = await _productService.UpdateProduct(product);
            if (product == null)
            {
                return Ok(ResponseHandle<Product>.Error("Update failse"));
            }
            return Ok(ResponseHandle<Product>.Success(product));
        }

        [HttpPost("product/CreateProductByBooking")]
        public async Task<IActionResult> CreateProductByBooking([FromBody] CreateProductByBookingRequest productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);
            product = await _productService.CreateProductByBooking(product);
            if (product == null)
            {
                return Ok(ResponseHandle<Product>.Error("Create product failse"));
            }
            return Ok(ResponseHandle<Product>.Success(product));
        }

        [HttpPost("product/updateProductByBooking")]
        public async Task<IActionResult> UpdateProductByBooking([FromBody] CreateProductByBookingRequest productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);
            product = await _productService.UpdateProductByBooking(product);
            if (product == null)
            {
                return Ok(ResponseHandle<Product>.Error("Create product failse"));
            }
            return Ok(ResponseHandle<Product>.Success(product));
        }

        [HttpGet("product/searchProduct")]
        public async Task<IActionResult> SearchProduct([FromQuery] SearchProductRequest filter)
        {
            var products = await _productService.SearchProduct(filter);
            if (products == null)
            {
                return Ok(ResponseHandle<Product>.Error("Do not have products which you choose."));
            }
            return Ok(ResponseArrayHandle<Product>.Success(products));
        }
    }
}
