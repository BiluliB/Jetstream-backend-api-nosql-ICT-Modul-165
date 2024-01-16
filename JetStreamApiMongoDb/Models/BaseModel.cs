using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Models
{
    public abstract class BaseModel
    {
        public static List<(string, string)> foreignKeys = new();

        [BsonId]
        public ObjectId Id { get; set; }
    }
}
