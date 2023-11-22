using BookStoreAPI.Entities.Dtos.BasketDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BasketValidators
{
    public class BasketItemDtoValidator : AbstractValidator<BasketItemDto>
    {
        public BasketItemDtoValidator()
        {
            RuleFor(basketItemDto => basketItemDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(basketItemDto => basketItemDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
            RuleFor(basketItemDto => basketItemDto.BookName).NotEmpty().WithMessage("BookName cannot be empty.");
            RuleFor(basketItemDto => basketItemDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
