using BookStoreAPI.Entities.Dtos.WishListsDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.WishListValidators
{
    public class WishListAddItemDtoValidator : AbstractValidator<WishListAddItemDto>
    {
        public WishListAddItemDtoValidator()
        {
            RuleFor(wishListAddItemDto => wishListAddItemDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
        }
    }
}
