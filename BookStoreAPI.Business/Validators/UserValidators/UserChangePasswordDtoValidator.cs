using BookStoreAPI.Entities.Dtos.UsersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.UserValidators
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(userChangePasswordDto => userChangePasswordDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(userChangePasswordDto => userChangePasswordDto.OldPassword).NotEmpty().WithMessage("OldPassword cannot be empty.");
            RuleFor(userChangePasswordDto => userChangePasswordDto.NewPassword).NotEmpty().WithMessage("NewPassword cannot be empty.");
        }
    }
}
