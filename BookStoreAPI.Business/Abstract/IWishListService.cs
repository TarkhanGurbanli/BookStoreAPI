using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.WishListsDto;

namespace BookStoreAPI.Business.Abstract
{
    public interface IWishListService
    {
        IResult AddWishList(string userId, WishListAddItemDto wishListAddItemDTO);
        IResult RemoveWishList(string userId, string productId);
        IDataResult<List<WishListItemDto>> GetUserWishList(string userId);
    }
}
