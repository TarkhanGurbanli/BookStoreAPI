using BookStoreAPI.Entities.Dtos.EmailDtos;

namespace BookStoreAPI.Core.Utilities.EmailHelper
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirm(string mailAddress, string token);
        Task<bool> SendEmailToAllUsers(EmailContentDto emailContent);
    }
}
