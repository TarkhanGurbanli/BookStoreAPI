using BookStoreAPI.Entities.Dtos.CategoriesDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.CategoryValidators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(categoryDto => categoryDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(categoryDto => categoryDto.CategoryName).NotEmpty().WithMessage("CategoryName cannot be empty.");
        }
    }
}
