using BookStoreAPI.Entities.Dtos.CategoriesDto;

namespace BookStoreAPI.Entities.Dtos.BooksDto
{
    public class BookDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public BookFeatureDto BookFeature { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
