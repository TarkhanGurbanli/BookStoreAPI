using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.BooksDto;

namespace BookStoreAPI.Business.Abstract
{
    public interface IBookService
    {
        Task<IDataResult<List<BookDto>>> GetAllBookAsync();
        Task<IDataResult<BookDto>> GetByIdBookAsync(string id);
        IResult BookChangeStatus(string BookId);
        Task<List<CategoryWithBooksDto>> GetAllCategoriesWithBooksAsync();
        Task<IResult> BookCreateAsync(BookCreateDto courseCreateDto);
        Task<IResult> BookUpdateAsync(BookUpdateDto courseUpdateDto);
        Task<IResult> BookDeleteAsync(string id);
    }
}
