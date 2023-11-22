using BookStoreAPI.Entities.Dtos.UsersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.UserValidators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(userUpdateDto => userUpdateDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(userUpdateDto => userUpdateDto.FirstName).NotEmpty().WithMessage("FirstName cannot be empty.");
            RuleFor(userUpdateDto => userUpdateDto.LastName).NotEmpty().WithMessage("LastName cannot be empty.");
            RuleFor(userUpdateDto => userUpdateDto.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
        }
    }
}
