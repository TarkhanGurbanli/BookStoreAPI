using BookStoreAPI.Entities.Dtos.WishListsDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.WishListValidators
{
    public class WishListItemDtoValidator : AbstractValidator<WishListItemDto>
    {
        public WishListItemDtoValidator()
        {
            RuleFor(wishListItemDto => wishListItemDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
            RuleFor(wishListItemDto => wishListItemDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(wishListItemDto => wishListItemDto.BookName).NotEmpty().WithMessage("BookName cannot be empty.");
        }
    }
}
