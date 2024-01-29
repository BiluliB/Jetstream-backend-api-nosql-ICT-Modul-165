using MongoDB.Bson;

namespace JetStreamApiMongoDb.Data
{

    public static class MongoDbSchema
    {
        public static BsonDocument OrderSubmissionSchema = new BsonDocument
    {
        { "bsonType", "object" },
        { "required", new BsonArray { "firstname", "lastname", "email", "phone", "priority_id", "create_date", "pickup_date", "service_id", "total_price_chf", "status_id" } },
        { "properties", new BsonDocument
            {
                { "firstname", new BsonDocument { { "bsonType", "string" }, { "maxLength", 50 } } },
                { "lastname", new BsonDocument { { "bsonType", "string" }, { "maxLength", 50 } } },
                { "email", new BsonDocument { { "bsonType", "string" } } },
                { "phone", new BsonDocument { { "bsonType", "string" } } },
                { "priority_id", new BsonDocument { { "bsonType", "objectId" } } },
                { "create_date", new BsonDocument { { "bsonType", "date" } } },
                { "pickup_date", new BsonDocument { { "bsonType", "date" } } },
                { "service_id", new BsonDocument { { "bsonType", "objectId" } } },
                { "total_price_chf", new BsonDocument { { "bsonType", "double" } } },
                { "status_id", new BsonDocument { { "bsonType", "objectId" } } },
                { "comment", new BsonDocument { { "bsonType", "string" }, { "maxLength", 500 } } },
                { "user_id", new BsonDocument { { "bsonType", new BsonArray { "objectId", "null" } } } }
            }
        }
    };

        public static BsonDocument PrioritySchema = new BsonDocument
    {
        { "bsonType", "object" },
        { "required", new BsonArray { "name", "price" } },
        { "properties", new BsonDocument
            {
                { "name", new BsonDocument { { "bsonType", "string" }, { "maxLength", 50 } } },
                { "price", new BsonDocument { { "bsonType", "double" } } }
            }
        }
    };

        public static BsonDocument ServiceSchema = new BsonDocument
    {
        { "bsonType", "object" },
        { "required", new BsonArray { "name", "price" } },
        { "properties", new BsonDocument
            {
                { "name", new BsonDocument { { "bsonType", "string" }, { "maxLength", 50 } } },
                { "price", new BsonDocument { { "bsonType", "double" } } }
            }
        }
    };

        public static BsonDocument StatusSchema = new BsonDocument
        {
            {"bsonType", "object" },
            {"required", new BsonArray {"name"} },
            {"properties", new BsonDocument
            {
                    {"name", new BsonDocument {{"bsonType", "string"}, { "maxLength", 50}}}
                }
            }

          
        };

        public static BsonDocument UserSchema = new BsonDocument
    {
        { "bsonType", "object" },
        { "required", new BsonArray { "name", "password_hash", "password_salt" } },
        { "properties", new BsonDocument
            {
                { "name", new BsonDocument { { "bsonType", "string" } } },
                { "password_hash", new BsonDocument { { "bsonType", "binData" } } },
                { "password_salt", new BsonDocument { { "bsonType", "binData" } } }
            }
        }
    };
    }

}
