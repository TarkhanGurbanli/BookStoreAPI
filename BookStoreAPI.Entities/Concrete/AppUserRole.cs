using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookStoreAPI.Entities.Concrete
{
    public class AppUserRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
