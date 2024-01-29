using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a priority
    /// </summary>
    public class Priority : BaseModel
    {
        [BsonElement("name")]
        [MaxLength(50)]

        [AllowNull, NotNull]
        public string Name { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }
    }
}
