using AutoMapper;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.BooksDto;
using BookStoreAPI.Entities.Dtos.CategoriesDto;
using BookStoreAPI.Entities.Dtos.OrdersDto;
using BookStoreAPI.Entities.Dtos.RoleDtos;
using BookStoreAPI.Entities.Dtos.UsersDto;
using BookStoreAPI.Entities.Dtos.WishListsDto;

namespace BookStoreAPI.Business.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //--Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            //--Book
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookCreateDto>().ReverseMap();
            CreateMap<Book, BookUpdateDto>().ReverseMap();
            CreateMap<BookFeature, BookFeatureDto>().ReverseMap();

            //--User
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserChangePasswordDto>().ReverseMap();
            CreateMap<UserLoginDto, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName)).ReverseMap();

            //--Order
            CreateMap<OrderCreateDTO, Order>().ReverseMap();
            CreateMap<Order , BookDecrementQuantityDTO>().ReverseMap();
            CreateMap<Order, OrderUserDto>()
           .ForMember(x => x.BooktId, o => o.MapFrom(s => s.Book.Id))
           .ForMember(x => x.Quantity, o => o.MapFrom(s => s.Quantity))
           .ForMember(x => x.Price, o => o.MapFrom(s => s.Book.Price));

            //--WishList
            CreateMap<WishListAddItemDto, WishList>().ReverseMap();
            CreateMap<WishList, WishListItemDto>().ReverseMap();
            CreateMap<WishList, WishListDto>().ReverseMap();

            //--Role
            CreateMap<AppRole, RoleCreateDto>().ReverseMap();
            CreateMap<AppRole, RoleUpdateDto>().ReverseMap();

            //--AppUserRole
            CreateMap<AppUserRole, UserRoleCreateDto>().ReverseMap();
            CreateMap<AppUserRole, UserRoleDeleteDto>().ReverseMap();








        }
    }
}
