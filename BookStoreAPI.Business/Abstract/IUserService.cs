using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.OrdersDto;
using BookStoreAPI.Entities.Dtos.UsersDto;
using BookStoreAPI.Entities.Dtos.WishListsDto;

namespace BookStoreAPI.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<UserDto>> GetUserByIdAsync(Guid userId);
        Task<IResult> CreateUserAsync(UserRegisterDto userCreateDto);
        Task<IResult> LoginAsync(UserLoginDto userLoginDto);
        Task<IResult> EmailVerifyAsync(string email, string verifyToken);
        Task<IResult> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<IResult> DeleteUserAsync(Guid userId);
        Task<IResult> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto);
        Task<IDataResult<List<UserDto>>> GetAllUsersAsync();
        Task<IDataResult<List<WishListDto>>> GetUserWishListAsync(string userId);
    }
}
