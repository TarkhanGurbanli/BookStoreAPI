using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreAPI.Entities.Concrete
{
    public class AppRole : MongoIdentityRole<Guid>
    {

        [BsonElement("RoleName")] 
        public string RoleName { get; set; }
    }
}
