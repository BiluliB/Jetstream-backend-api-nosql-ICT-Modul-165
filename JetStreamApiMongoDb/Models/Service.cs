using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a service
    /// </summary>
    public class Service : BaseModel
    {
        [BsonElement("name")]
        [MaxLength(50)]

        [AllowNull, NotNull]
        public string Name { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }
    }
}
