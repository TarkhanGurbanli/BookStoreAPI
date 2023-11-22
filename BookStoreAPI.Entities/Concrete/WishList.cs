using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookStoreAPI.Entities.Concrete
{
    public class WishList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
