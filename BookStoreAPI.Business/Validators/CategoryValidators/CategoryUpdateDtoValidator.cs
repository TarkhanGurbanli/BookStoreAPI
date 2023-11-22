using BookStoreAPI.Entities.Dtos.CategoriesDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.CategoryValidators
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(categoryUpdateDto => categoryUpdateDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(categoryUpdateDto => categoryUpdateDto.CategoryName).NotEmpty().WithMessage("CategoryName cannot be empty.");
        }
    }
}
