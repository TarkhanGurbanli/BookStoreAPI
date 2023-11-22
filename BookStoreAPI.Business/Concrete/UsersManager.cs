using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.EmailHelper;
using BookStoreAPI.Core.Utilities.JWT;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.UsersDto;
using BookStoreAPI.Entities.Dtos.WishListsDto;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace BookStoreAPI.Business.Concrete
{
    public class UsersManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMongoCollection<AppUser> _userCollection;
        private readonly IMongoCollection<WishList> _wishListCollection;
        private readonly IMapper _mapper;
        public UsersManager(UserManager<AppUser> userManager, IMapper mapper, IDatabaseSettings databaseSettings, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            // MongoDB bağlantısı ucun Lazim olan koleksiyonları al
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<AppUser>(databaseSettings.UserCollectionName);
            _wishListCollection = database.GetCollection<WishList>(databaseSettings.WishListCollectionName);
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<IResult> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                var existingUser = await _userCollection.Find(x => x.Id == userChangePasswordDto.Id).FirstOrDefaultAsync();

                if (existingUser == null || existingUser.Id != userChangePasswordDto.Id)
                {
                    return new ErrorResult("User not found");
                }

                existingUser.Password = userChangePasswordDto.NewPassword;

                await _userCollection.ReplaceOneAsync(x => x.Id == userChangePasswordDto.Id, existingUser);

                var result = await _userManager.ChangePasswordAsync(existingUser, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);

                if (result.Succeeded)
                    return new SuccessResult("Password changed successfully");

                return new ErrorResult($"Password change failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while changing the password: {ex.Message}");
            }
        }

        public async Task<IResult> CreateUserAsync(UserRegisterDto userCreateDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userCreateDto.Email);

                if (existingUser != null)
                {
                    if (!existingUser.EmailConfirmed)
                    {
                        return new ErrorResult("The email address is already registered but not verified.");
                    }
                    return new ErrorResult("The email address is already registered and verified.");
                }

                var userToCreate = _mapper.Map<AppUser>(userCreateDto);

                userToCreate.Token = Guid.NewGuid().ToString();
                userToCreate.TokenExpiresDate = DateTime.Now.AddDays(3); 

                IdentityResult result = await _userManager.CreateAsync(userToCreate, userCreateDto.Password);

                if (result.Succeeded)
                {
                    await _userCollection.InsertOneAsync(userToCreate);

                    var emailResult = await _emailService.SendEmailConfirm(userToCreate.Email, userToCreate.Token);

                    if (emailResult)
                        return new SuccessResult("User created successfully");

                    return new ErrorResult("Email verification sending error");
                }

                return new ErrorResult($"An error occurred while creating the user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while creating the user: {ex.Message}");
            }
        }

        public async Task<IResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

                if (user == null)
                    return new ErrorResult("User not found");

                await _userCollection.DeleteOneAsync(x => x.Id == userId);

                var result = await _userManager.DeleteAsync(user);

                return result.Succeeded
                    ? new SuccessResult("User deleted successfully")
                    : new ErrorResult($"An error occurred while deleting the user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while deleting the user: {ex.Message}");
            }
        }

        public async Task<IDataResult<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userCollection.Find(_ => true).ToListAsync();

                if (users == null || !users.Any())
                    return new ErrorDataResult<List<UserDto>>("No users found");

                var userDtos = _mapper.Map<List<UserDto>>(users);

                return new SuccessDataResult<List<UserDto>>(userDtos, "Users retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserDto>>($"An error occurred while getting all users: {ex.Message}");
            }
        }

        public async Task<IDataResult<UserDto>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

                var userDto = _mapper.Map<UserDto>(user);

                if (user == null || user.Id != userDto.Id)
                    return new ErrorDataResult<UserDto>("User not found");

                return new SuccessDataResult<UserDto>(userDto, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserDto>($"An error occurred while getting the user: {ex.Message}");
            }
        }

        public async Task<IResult> LoginAsync(UserLoginDto userLoginDto)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(userLoginDto.UserName);
                var token = Token.TokenGenerator(existingUser, "User");
                if (existingUser == null)
                    return new ErrorResult("User not found");

                var result = await _signInManager.PasswordSignInAsync(existingUser, userLoginDto.Password, false, false);

                if (result.Succeeded)
                    return new SuccessResult(token);

                return new ErrorResult("Invalid login attempt");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred during login: {ex.Message}");
            }
        }

        public async Task<IDataResult<List<WishListDto>>> GetUserWishListAsync(string userId)
        {
            try
            {
                var wishList = await _wishListCollection.Find(x => x.UserId == userId).ToListAsync();

                if (wishList == null || !wishList.Any())
                    return new ErrorDataResult<List<WishListDto>>("No wish list found for the user");

                var wishListDtos = _mapper.Map<List<WishListDto>>(wishList);

                return new SuccessDataResult<List<WishListDto>>(wishListDtos, "User wish list retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<WishListDto>>($"An error occurred while getting user wish list: {ex.Message}");
            }
        }

        public async Task<IResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var updateUser = await _userCollection.FindOneAndUpdateAsync(
                    Builders<AppUser>.Filter.Eq(x => x.Id, userUpdateDto.Id),
                    Builders<AppUser>.Update
                        .Set(x => x.FirstName, userUpdateDto.FirstName)
                        .Set(x => x.LastName, userUpdateDto.LastName)
                        .Set(x => x.UserName, userUpdateDto.UserName)
                        .Set(x => x.NormalizedUserName, userUpdateDto.NormalizedUserName)
                );

                if (updateUser == null)
                    return new ErrorResult("User not found");

                return new SuccessResult("User updated successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while updating the user: {ex.Message}");
            }
        }

        public async Task<IResult> EmailVerifyAsync(string email, string verifyToken)
        {
            try
            {
                var userToVerify = await _userManager.FindByEmailAsync(email);

                if (userToVerify == null)
                    return new ErrorResult("User not found");

                if (!string.IsNullOrEmpty(verifyToken) && userToVerify.Token == verifyToken && DateTime.Compare(userToVerify.TokenExpiresDate, DateTime.Now) > 0)
                {
                    userToVerify.EmailConfirmed = true;
                    await _userManager.UpdateAsync(userToVerify);

                    return new SuccessResult("Email verification successful");
                }

                if (DateTime.Compare(userToVerify.TokenExpiresDate, DateTime.Now) <= 0)
                    return new ErrorResult("The key has expired");

                return new ErrorResult("Invalid verification key");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred during email verification: {ex.Message}");
            }
        }
    }
}
