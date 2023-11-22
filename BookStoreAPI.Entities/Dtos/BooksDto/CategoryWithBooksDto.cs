using BookStoreAPI.Entities.Dtos.CategoriesDto;

namespace BookStoreAPI.Entities.Dtos.BooksDto
{
    public class CategoryWithBooksDto
    {
        public CategoryDto Category { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
