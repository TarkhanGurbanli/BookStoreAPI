using BookStoreAPI.Entities.Dtos.BasketDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.BasketValidators
{
    public class BasketDtoValidator : AbstractValidator<BasketDto>
    {
        public BasketDtoValidator()
        {
            RuleFor(basketDto => basketDto.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
            RuleFor(basketDto => basketDto.basketItems).NotNull().WithMessage("BasketItems cannot be null.");
            RuleForEach(basketDto => basketDto.basketItems).SetValidator(new BasketItemDtoValidator()).When(basketDto => basketDto.basketItems != null);
        }
    }
}
