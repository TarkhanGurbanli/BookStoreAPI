using BookStoreAPI.Entities.Dtos.PhotoDtos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreAPI.Entities.Concrete
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedDate { get; set; }
        public bool IsFeatured { get; set; }
        public BookFeature BookFeature { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        [BsonIgnore]
        public Category Category { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<Order> Orders { get; set; }
        public List<PhotoDto> Photos { get; set; } = new List<PhotoDto>();
    }
}
