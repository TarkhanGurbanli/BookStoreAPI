using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
    {
        public BookUpdateDtoValidator()
        {
            RuleFor(bookUpdateDto => bookUpdateDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(bookUpdateDto => bookUpdateDto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(bookUpdateDto => bookUpdateDto.Description).NotEmpty().WithMessage("Description cannot be empty.");
            RuleFor(bookUpdateDto => bookUpdateDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(bookUpdateDto => bookUpdateDto.Picture).NotEmpty().WithMessage("Picture cannot be empty.");
            RuleFor(bookUpdateDto => bookUpdateDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(bookUpdateDto => bookUpdateDto.UpdatedDate).NotEmpty().WithMessage("UpdatedDate cannot be empty.");
            RuleFor(bookUpdateDto => bookUpdateDto.BookFeature).SetValidator(new BookFeatureDtoValidator()).When(bookUpdateDto => bookUpdateDto.BookFeature != null);
            RuleFor(bookUpdateDto => bookUpdateDto.CategoryId).NotEmpty().WithMessage("CategoryId cannot be empty.");
        }
    }
}
