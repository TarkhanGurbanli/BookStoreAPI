using BookStoreAPI.Entities.Dtos.RoleDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.RoleValidators
{
    public class UserRoleCreateDtoValidator : AbstractValidator<UserRoleCreateDto>
    {
        public UserRoleCreateDtoValidator()
        {
            RuleFor(userRoleCreateDto => userRoleCreateDto.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
            RuleFor(userRoleCreateDto => userRoleCreateDto.RoleId).NotEmpty().WithMessage("RoleId cannot be empty.");
        }
    }
}
