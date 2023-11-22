using BookStoreAPI.Entities.Dtos.PhotoDtos;
using FluentValidation;

namespace BookStoreAPI.Business.Validators.PhotoValidators
{
    public class PhotoDtoValidator : AbstractValidator<PhotoDto>
    {
        public PhotoDtoValidator()
        {
            RuleFor(photoDto => photoDto.Url).NotEmpty().WithMessage("Url cannot be empty.");
        }
    }
}
