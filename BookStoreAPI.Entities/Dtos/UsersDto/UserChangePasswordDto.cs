using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.Dtos.UsersDto
{
    public class UserChangePasswordDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
