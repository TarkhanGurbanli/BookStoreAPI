using BookStoreAPI.Entities.Dtos.UsersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.UserValidators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(userDto => userDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(userDto => userDto.FirstName).NotEmpty().WithMessage("FirstName cannot be empty.");
            RuleFor(userDto => userDto.LastName).NotEmpty().WithMessage("LastName cannot be empty.");
            RuleFor(userDto => userDto.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
            RuleFor(userDto => userDto.Email).NotEmpty().WithMessage("Email cannot be empty.").EmailAddress().WithMessage("Invalid email address.");
        }
    }
}
