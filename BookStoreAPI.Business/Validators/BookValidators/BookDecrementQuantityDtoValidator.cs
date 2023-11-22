using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class BookDecrementQuantityDtoValidator : AbstractValidator<BookDecrementQuantityDTO>
    {
        public BookDecrementQuantityDtoValidator()
        {
            RuleFor(bookDecrementQuantityDto => bookDecrementQuantityDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
            RuleFor(bookDecrementQuantityDto => bookDecrementQuantityDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
