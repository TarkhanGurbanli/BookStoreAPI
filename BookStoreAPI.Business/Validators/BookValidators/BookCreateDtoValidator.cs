using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(bookCreateDto => bookCreateDto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(bookCreateDto => bookCreateDto.Description).NotEmpty().WithMessage("Description cannot be empty.");
            RuleFor(bookCreateDto => bookCreateDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(bookCreateDto => bookCreateDto.Picture).NotEmpty().WithMessage("Picture cannot be empty.");
            RuleFor(bookCreateDto => bookCreateDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(bookCreateDto => bookCreateDto.CreatedDate).NotEmpty().WithMessage("CreatedDate cannot be empty.");
            RuleFor(bookCreateDto => bookCreateDto.BookFeature).SetValidator(new BookFeatureDtoValidator()).When(bookCreateDto => bookCreateDto.BookFeature != null);
            RuleFor(bookCreateDto => bookCreateDto.CategoryId).NotEmpty().WithMessage("CategoryId cannot be empty.");
        }
    }
}
