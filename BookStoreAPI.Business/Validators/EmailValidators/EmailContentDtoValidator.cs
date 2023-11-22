using BookStoreAPI.Entities.Dtos.EmailDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.EmailValidators
{
    public class EmailContentDtoValidator : AbstractValidator<EmailContentDto>
    {
        public EmailContentDtoValidator()
        {
            RuleFor(emailContentDto => emailContentDto.Subject).NotEmpty().WithMessage("Subject cannot be empty.");
            RuleFor(emailContentDto => emailContentDto.Body).NotEmpty().WithMessage("Body cannot be empty.");
        }
    }
}
