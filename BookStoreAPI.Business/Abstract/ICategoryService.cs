using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.BooksDto;
using BookStoreAPI.Entities.Dtos.CategoriesDto;

namespace BookStoreAPI.Business.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<List<CategoryDto>>> GetAllCategoryAsync();
        Task<IDataResult<CategoryWithBooksDto>> GetSingleCategoryByIdWithProductsAsync(string categoryId);
        Task<IResult> CategoryCreateAsync(CategoryCreateDto categoryCreateDto);
        Task<IDataResult<CategoryDto>> GetByIdCategoryAsync(string id);
        Task<IResult> CategoryRemoveAsync(string id);
        Task<IResult> CategoryUpdateAsync(CategoryUpdateDto categoryUpdateDto);
    }
}
