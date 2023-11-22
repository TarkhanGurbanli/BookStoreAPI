using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Dtos.BooksDto;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("getBookById/{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            var response = await _bookService.GetByIdBookAsync(id);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("getBookAll")]
        public async Task<IActionResult> GetAllBook()
        {
            var response = await _bookService.GetAllBookAsync();
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpGet("allCategoriesWithBooks")]
        public async Task<IActionResult> GetAllCategoriesWithBooks()
        {
            var result = await _bookService.GetAllCategoriesWithBooksAsync();

            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }

            return NotFound("Categories with books not found.");
        }

        [HttpPost("createBook")]
        public async Task<IActionResult> CreateBook(BookCreateDto bookCreateDto)
        {
            var newBook = await _bookService.BookCreateAsync(bookCreateDto);
            if (newBook.Success)
                return Ok(newBook);

            return BadRequest(newBook);
        }

        [HttpPut("updateBook")]
        public async Task<IActionResult> UpdateBook(BookUpdateDto bookUpdateDto)
        {
            var updateBook = await _bookService.BookUpdateAsync(bookUpdateDto);
            if (updateBook.Success)
                return Ok(updateBook);

            return BadRequest(updateBook);
        }

        [HttpPatch("changeFeatureStatus/{bookId}")]
        public IActionResult ChangeFeatureStatus(string bookId)
        {
            var result = _bookService.BookChangeStatus(bookId);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpDelete("deleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var deleteBook = await _bookService.BookDeleteAsync(id);
            if (deleteBook.Success)
                return Ok(deleteBook);

            return BadRequest(deleteBook);
        }
    }
}
