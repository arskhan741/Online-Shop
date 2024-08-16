using Microsoft.AspNetCore.Mvc;
using Online_Shop.Contracts;
using Online_Shop.DTOs.CategoryDTOs;

namespace Online_Shop.Controllers
{
    [ApiController]
    [Route("Category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }


        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Category = await _categoryRepository.CreateAsync(createCategoryDTO);

            return Ok(Category);
        }

        [HttpGet]
        [Route("GetCategory/{categoryId:int}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryRepository.GetAsync(categoryId);

            return Ok(category);
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var catagories = await _categoryRepository.GetAllAsync();
            return Ok(catagories);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryDTO deleteCategoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryRepository.DeleteAsync(deleteCategoryDTO);

            return Ok(new { message = $"Category with id: {deleteCategoryDTO.CategoryId} is deleted" });
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _categoryRepository.UpdateAsync(updateCategoryDTO);
            return Ok(category);
        }

    }
}
