using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Business.Concrete;
using BookStoreAPI.Business.Validators.BasketValidators;
using BookStoreAPI.Business.Validators.BookValidators;
using BookStoreAPI.Business.Validators.CategoryValidators;
using BookStoreAPI.Business.Validators.EmailValidators;
using BookStoreAPI.Business.Validators.OrderValidators;
using BookStoreAPI.Business.Validators.PhotoValidators;
using BookStoreAPI.Business.Validators.RoleValidators;
using BookStoreAPI.Business.Validators.UserValidators;
using BookStoreAPI.Business.Validators.WishListValidators;
using BookStoreAPI.Entities.Dtos.BasketDtos;
using BookStoreAPI.Entities.Dtos.BooksDto;
using BookStoreAPI.Entities.Dtos.CategoriesDto;
using BookStoreAPI.Entities.Dtos.EmailDtos;
using BookStoreAPI.Entities.Dtos.OrdersDto;
using BookStoreAPI.Entities.Dtos.PhotoDtos;
using BookStoreAPI.Entities.Dtos.RoleDtos;
using BookStoreAPI.Entities.Dtos.UsersDto;
using BookStoreAPI.Entities.Dtos.WishListsDto;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreAPI.Business.DependencyResolvers
{
    public static class ServiceRegistration
    {
        public static void Run(this IServiceCollection service)
        {
            //--Category
            service.AddScoped<ICategoryService, CategoryManager>();

            //--Book
            service.AddScoped<IBookService, BookManager>();

            //--User
            service.AddScoped<IUserService, UsersManager>();

            //--Photos
            service.AddScoped<IPhotoStockService, PhotoStockManager>();

            //--WishList
            service.AddScoped<IWishListService, WishListManager>();

            //--Order
            service.AddScoped<IOrderService, OrderManager>();

            //--Role
            service.AddScoped<IRoleService, RoleManager>();

            //--Basket
            service.AddScoped<IBasketService, BasketManager>();

            //--BasketValidator
            service.AddScoped<IValidator<BasketDto>, BasketDtoValidator>();
            service.AddScoped<IValidator<BasketItemDto>, BasketItemDtoValidator>();

            //--BookValidator
            service.AddScoped<IValidator<BookCreateDto>, BookCreateDtoValidator>();
            service.AddScoped<IValidator<BookDecrementQuantityDTO>, BookDecrementQuantityDtoValidator>();
            service.AddScoped<IValidator<BookDto>, BookDtoValidator>();
            service.AddScoped<IValidator<BookFeatureDto>, BookFeatureDtoValidator>();
            service.AddScoped<IValidator<BookUpdateDto>, BookUpdateDtoValidator>();
            service.AddScoped<IValidator<CategoryWithBooksDto>, CategoryWithBooksDtoValidator>();

            //--CategoryValidator
            service.AddScoped<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
            service.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
            service.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();

            //--EmailValidator
            service.AddScoped<IValidator<EmailContentDto>, EmailContentDtoValidator>();

            //--OrderValidator
            service.AddScoped<IValidator<OrderCreateDTO>, OrderCreateDtoValidator>();
            service.AddScoped<IValidator<OrderUserDto>, OrderUserDtoValidator>();
            service.AddScoped<IValidator<UserOrderDto>, UserOrderDtoValidator>();

            //--PhotoValidator
            service.AddScoped<IValidator<PhotoDto>, PhotoDtoValidator>();

            //--RoleValidator
            service.AddScoped<IValidator<RoleCreateDto>, RoleCreateDtoValidator>();
            service.AddScoped<IValidator<RoleUpdateDto>, RoleUpdateDtoValidator>();
            service.AddScoped<IValidator<UserRoleCreateDto>, UserRoleCreateDtoValidator>();
            service.AddScoped<IValidator<UserRoleDeleteDto>, UserRoleDeleteDtoValidator>();

            //--UserValidator
            service.AddScoped<IValidator<UserDto>, UserDtoValidator>();
            service.AddScoped<IValidator<UserChangePasswordDto>, UserChangePasswordDtoValidator>();
            service.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
            service.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>();
            service.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();

            //--WishListValidator
            service.AddScoped<IValidator<WishListAddItemDto>, WishListAddItemDtoValidator>();
            service.AddScoped<IValidator<WishListDto>, WishListDtoValidator>();
            service.AddScoped<IValidator<WishListItemDto>, WishListItemDtoValidator>();
        }
    }
}
