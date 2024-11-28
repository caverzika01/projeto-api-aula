using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _2TDSPK.Database.Models
{
    [Collection("products")]
    public class Product
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        [Required]
        public string Name { get; set; }

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Decimal128)]
        [Required]
        public decimal Price { get; set; }

        [BsonElement("stock")]
        [Required]
        public int Stock { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("category")]
        [Required]
        public string Category { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
