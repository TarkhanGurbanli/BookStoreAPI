namespace BookStoreAPI.Entities.Dtos.OrdersDto
{
    public class UserOrderDto
    {
        public string Id { get; set; }
        public List<OrderUserDto> OrderUserDTOs { get; set; }
    }
}
