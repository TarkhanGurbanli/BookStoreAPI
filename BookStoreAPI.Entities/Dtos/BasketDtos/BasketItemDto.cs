namespace BookStoreAPI.Entities.Dtos.BasketDtos
{
    public class BasketItemDto
    {
        public int Quantity { get; set; }
        public string BookId { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
    }
}
