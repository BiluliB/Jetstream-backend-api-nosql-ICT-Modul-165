using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a service
    /// </summary>
    public class Service : BaseModel
    {
        

        [BsonElement("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
