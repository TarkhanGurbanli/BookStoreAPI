namespace BookStoreAPI.Entities.Dtos.OrdersDto
{
    public class OrderCreateDTO
    {
        public string BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
