using BookStoreAPI.Entities.Dtos.WishListsDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.WishListValidators
{
    public class WishListDtoValidator : AbstractValidator<WishListDto>
    {
        public WishListDtoValidator()
        {
            RuleFor(wishListDto => wishListDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(wishListDto => wishListDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
            RuleFor(wishListDto => wishListDto.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
        }
    }
}
