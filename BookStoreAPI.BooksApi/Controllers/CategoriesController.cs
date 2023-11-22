using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Dtos.CategoriesDto;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            if (categories.Success)
                return Ok(categories);

            return BadRequest();
        }

        [HttpGet("getCategoryWithBooks/{categoryId}")]
        public async Task<IActionResult> GetCategoryWithBooks(string categoryId)
        {
            var result = await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.Message);
        }

        [HttpGet("getCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var categories = await _categoryService.GetByIdCategoryAsync(id);
            if (categories.Success)
                return Ok(categories);

            return BadRequest();
        }

        [HttpPost("createCategory")]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var response = await _categoryService.CategoryCreateAsync(categoryCreateDto);
            if (response.Success)
                return Ok(response);

            return BadRequest();
        }

        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var category = await _categoryService.CategoryUpdateAsync(categoryUpdateDto);
            if (category.Success)
                return Ok(category);

            return BadRequest();
        }

        [HttpDelete("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _categoryService.CategoryRemoveAsync(id);
            if (category.Success)
                return Ok(category);

            return BadRequest();
        }
    }
}
