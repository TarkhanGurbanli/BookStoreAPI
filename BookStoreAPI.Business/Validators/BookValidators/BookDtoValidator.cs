using BookStoreAPI.Business.Validators.CategoryValidators;
using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(bookDto => bookDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(bookDto => bookDto.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(bookDto => bookDto.Description).NotEmpty().WithMessage("Description cannot be empty.");
            RuleFor(bookDto => bookDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(bookDto => bookDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(bookDto => bookDto.Picture).NotEmpty().WithMessage("Picture cannot be empty.");
            RuleFor(bookDto => bookDto.CreatedTime).NotEmpty().WithMessage("CreatedTime cannot be empty.");
            RuleFor(bookDto => bookDto.BookFeature).SetValidator(new BookFeatureDtoValidator()).When(bookDto => bookDto.BookFeature != null);
            RuleFor(bookDto => bookDto.CategoryId).NotEmpty().WithMessage("CategoryId cannot be empty.");
            RuleFor(bookDto => bookDto.Category).SetValidator(new CategoryDtoValidator()).When(bookDto => bookDto.Category != null);
        }
    }
}
