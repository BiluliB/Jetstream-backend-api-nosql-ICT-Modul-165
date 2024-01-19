using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
