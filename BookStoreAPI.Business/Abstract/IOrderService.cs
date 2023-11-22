using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Entities.Dtos.OrdersDto;

namespace BookStoreAPI.Business.Abstract
{
    public interface IOrderService
    {
        IResult CreateOrder(string userId, List<OrderCreateDTO> orderCreateDTOs);
        IResult CancelOrder(string userId, string orderId);
        IDataResult<UserOrderDto> GetOrdersByUser(string userId);
    }
}
