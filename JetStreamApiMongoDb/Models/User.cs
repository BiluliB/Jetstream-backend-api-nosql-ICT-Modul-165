using JetStreamApiMongoDb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.Models
{
    public class User : BaseModel
    {
        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("password_hash")]
        [BsonRepresentation(BsonType.Binary)]
        [AllowNull, NotNull]
        public byte[] PasswordHash { get; set; }

        [BsonElement("password_salt")]
        [BsonRepresentation(BsonType.Binary)]
        [AllowNull, NotNull]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("password_input_attempt")]
        public int PasswordInputAttempt { get; set; } = 0;

        [BsonElement("is_locked")]
        public bool IsLocked { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Roles Role { get; set; }
    }
}
