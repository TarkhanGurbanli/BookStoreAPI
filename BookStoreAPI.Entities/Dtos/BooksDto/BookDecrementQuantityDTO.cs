namespace BookStoreAPI.Entities.Dtos.BooksDto
{
    public class BookDecrementQuantityDTO
    {
        public string BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
