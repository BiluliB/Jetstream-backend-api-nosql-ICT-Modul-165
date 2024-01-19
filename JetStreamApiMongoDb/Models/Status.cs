using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a status
    /// </summary>
    public class Status : BaseModel
    {
        [BsonElement("name")]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
