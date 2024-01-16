using JetStreamApiMongoDb.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a priority
    /// </summary>
    public class Priority : BaseModel
    {
        

        [BsonElement("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
