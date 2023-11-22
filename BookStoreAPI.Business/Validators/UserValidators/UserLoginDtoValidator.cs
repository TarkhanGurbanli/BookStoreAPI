using BookStoreAPI.Entities.Dtos.UsersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.UserValidators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(userLoginDto => userLoginDto.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
            RuleFor(userLoginDto => userLoginDto.Password).NotEmpty().WithMessage("Password cannot be empty.");
        }
    }
}
