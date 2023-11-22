using BookStoreAPI.Entities.Dtos.CategoriesDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.CategoryValidators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(categoryCreateDto => categoryCreateDto.CategoryName).NotEmpty().WithMessage("CategoryName cannot be empty.");
        }
    }
}
