using BookStoreAPI.Entities.Dtos.OrdersDto;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.OrderValidators
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(orderCreateDto => orderCreateDto.BookId).NotEmpty().WithMessage("BookId cannot be empty.");
            RuleFor(orderCreateDto => orderCreateDto.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(orderCreateDto => orderCreateDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
