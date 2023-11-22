using BookStoreAPI.Entities.Concrete;

namespace BookStoreAPI.Entities.Dtos.BooksDto
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }
        public BookFeatureDto BookFeature { get; set; }
        public string CategoryId { get; set; }
    }
}
