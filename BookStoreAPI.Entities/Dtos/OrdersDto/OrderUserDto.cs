namespace BookStoreAPI.Entities.Dtos.OrdersDto
{
    public class OrderUserDto
    {
        public string BooktId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
