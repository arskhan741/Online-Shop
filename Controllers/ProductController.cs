using Microsoft.AspNetCore.Mvc;
using Online_Shop.Contracts;
using Online_Shop.DTOs.ProductDTOs;

namespace Online_Shop.Controllers
{
    [ApiController]
    [Route("api/ProductController")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.CreateAsync(createProductDTO);

            return Ok(product);
        }

        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.GetAsync(productId);

            return Ok(product);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(DeleteProductDTO deleteProductDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productRepository.DeleteAsync(deleteProductDTO);

            return Ok(new { message = $"Product with id:{deleteProductDTO.ProductId} deleted" });
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            var product = await _productRepository.UpdateAsync(updateProductDTO);
            return Ok(product);
        }

    }
}
