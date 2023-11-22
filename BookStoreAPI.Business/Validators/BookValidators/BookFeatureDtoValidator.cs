using BookStoreAPI.Entities.Dtos.BooksDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BookValidators
{
    public class BookFeatureDtoValidator : AbstractValidator<BookFeatureDto>
    {
        public BookFeatureDtoValidator()
        {
            RuleFor(bookFeatureDto => bookFeatureDto.Author).NotEmpty().WithMessage("Author cannot be empty.");
            RuleFor(bookFeatureDto => bookFeatureDto.Language).NotEmpty().WithMessage("Language cannot be empty.");
            RuleFor(bookFeatureDto => bookFeatureDto.NumberOfpages).GreaterThan(0).WithMessage("NumberOfPages must be greater than 0.");
            RuleFor(bookFeatureDto => bookFeatureDto.Cover).NotEmpty().WithMessage("Cover cannot be empty.");
            RuleFor(bookFeatureDto => bookFeatureDto.Publisher).NotEmpty().WithMessage("Publisher cannot be empty.");
        }
    }
}
