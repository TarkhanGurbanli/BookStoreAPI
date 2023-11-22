using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.BasketDtos;

namespace BookStoreAPI.Business.Abstract
{
    public interface IBasketService
    {
        Task<IDataResult<BasketDto>> GetBasket(string userId);
        Task<IResult> AddOrUpdateBasket(BasketDto basket);
        Task<IResult> DeleteAsync(string userId, string bookId);
    }
}
