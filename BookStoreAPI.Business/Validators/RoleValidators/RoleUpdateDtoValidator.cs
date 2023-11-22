using BookStoreAPI.Entities.Dtos.RoleDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.RoleValidators
{
    public class RoleUpdateDtoValidator : AbstractValidator<RoleUpdateDto>
    {
        public RoleUpdateDtoValidator()
        {
            RuleFor(roleUpdateDto => roleUpdateDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(roleUpdateDto => roleUpdateDto.RoleName).NotEmpty().WithMessage("RoleName cannot be empty.");
        }
    }
}
