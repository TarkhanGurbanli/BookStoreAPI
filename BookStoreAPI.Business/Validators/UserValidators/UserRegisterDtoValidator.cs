using BookStoreAPI.Entities.Dtos.UsersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.UserValidators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(userRegisterDto => userRegisterDto.FirstName).NotEmpty().WithMessage("FirstName cannot be empty.");
            RuleFor(userRegisterDto => userRegisterDto.LastName).NotEmpty().WithMessage("LastName cannot be empty.");
            RuleFor(userRegisterDto => userRegisterDto.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
            RuleFor(userRegisterDto => userRegisterDto.Email).NotEmpty().WithMessage("Email cannot be empty.").EmailAddress().WithMessage("Invalid email address.");
            RuleFor(userRegisterDto => userRegisterDto.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
