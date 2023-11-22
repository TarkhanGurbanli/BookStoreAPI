using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class CategoryWithBooksDtoValidator : AbstractValidator<CategoryWithBooksDto>
    {
        public CategoryWithBooksDtoValidator()
        {
            RuleFor(categoryWithBooksDto => categoryWithBooksDto.Category).NotNull().WithMessage("Category cannot be null.");
            RuleFor(categoryWithBooksDto => categoryWithBooksDto.Books).NotNull().WithMessage("Books cannot be null.");
            RuleForEach(categoryWithBooksDto => categoryWithBooksDto.Books).SetValidator(new BookDtoValidator()).When(categoryWithBooksDto => categoryWithBooksDto.Books != null);
        }
    }
}
