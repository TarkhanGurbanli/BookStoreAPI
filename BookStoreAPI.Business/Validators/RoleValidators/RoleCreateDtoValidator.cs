using BookStoreAPI.Entities.Dtos.RoleDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.RoleValidators
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateDtoValidator()
        {
            RuleFor(roleCreateDto => roleCreateDto.RoleName).NotEmpty().WithMessage("RoleName cannot be empty.");
        }
    }
}
