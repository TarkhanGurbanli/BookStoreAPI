using BookStoreAPI.Entities.Dtos.OrdersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.OrderValidators
{
    public class OrderUserDtoValidator : AbstractValidator<OrderUserDto>
    {
        public OrderUserDtoValidator()
        {
            RuleFor(orderUserDto => orderUserDto.BooktId).NotEmpty().WithMessage("BooktId cannot be empty.");
            RuleFor(orderUserDto => orderUserDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(orderUserDto => orderUserDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
