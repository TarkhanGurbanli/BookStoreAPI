using BookStoreAPI.Entities.Dtos.OrdersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.OrderValidators
{
    public class UserOrderDtoValidator : AbstractValidator<UserOrderDto>
    {
        public UserOrderDtoValidator()
        {
            RuleFor(userOrderDto => userOrderDto.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(userOrderDto => userOrderDto.OrderUserDTOs).NotNull().WithMessage("OrderUserDTOs cannot be null.");
            RuleForEach(userOrderDto => userOrderDto.OrderUserDTOs).SetValidator(new OrderUserDtoValidator()).When(userOrderDto => userOrderDto.OrderUserDTOs != null);
        }
    }
}
