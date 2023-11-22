using BookStoreAPI.Entities.Dtos.RoleDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.RoleValidators
{
    public class UserRoleDeleteDtoValidator : AbstractValidator<UserRoleDeleteDto>
    {
        public UserRoleDeleteDtoValidator()
        {
            RuleFor(userRoleDeleteDto => userRoleDeleteDto.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
            RuleFor(userRoleDeleteDto => userRoleDeleteDto.RoleId).NotEmpty().WithMessage("RoleId cannot be empty.");
        }
    }
}
