using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.EmailHelper;
using BookStoreAPI.Entities.Dtos.EmailDtos;
using BookStoreAPI.Entities.Dtos.UsersDto;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UsersController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userCreateDto)
        {
            var newUser = await _userService.CreateUserAsync(userCreateDto);
            if (newUser.Success)
                return Ok(newUser);

            return BadRequest(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _userService.LoginAsync(userLoginDto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("sendToAllUsers")]
        public async Task<IActionResult> SendEmailToAllUsers([FromBody] EmailContentDto emailContent)
        {
            var result = await _emailService.SendEmailToAllUsers(emailContent);

            if (result)
                return Ok(new { success = true, message = "Emails sent successfully." });
            else
                return BadRequest(new { success = false, message = "Error sending emails." });
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            var updateUser = await _userService.UpdateUserAsync(userUpdateDto);
            if (updateUser.Success)
                return Ok(updateUser);

            return BadRequest(updateUser);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto userChangePasswordDto)
        {
            var result = await _userService.ChangePasswordAsync(userChangePasswordDto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleteUser = await _userService.DeleteUserAsync(id);
            if (deleteUser.Success)
                return Ok(deleteUser);

            return BadRequest(deleteUser);
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users.Success)
                return Ok(users);

            return BadRequest(users);
        }

        [HttpGet("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("verifypassword")]
        public async Task<IActionResult> VerifyPassword([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _userService.EmailVerifyAsync(email, token);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("getUserWishList/{userId}")]
        public async Task<IActionResult> GetUserWishList(string userId)
        {
            var result = await _userService.GetUserWishListAsync(userId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
