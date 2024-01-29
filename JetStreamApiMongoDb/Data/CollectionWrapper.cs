using AutoMapper;
using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;

namespace JetStreamApiMongoDb.Data
{
    public class CollectionWrapper<T>
        where T : BaseModel
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabase _database;

        public IMongoCollection<T> Collection => _collection;

        public CollectionWrapper(IMapper mapper, IMongoDatabase database, string collectionName)
        {
            _mapper = mapper;
            _database = database;
            var collectionList = _database.ListCollectionNames().ToList();

            if (!collectionList.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
                ApplyCollectionSchema(collectionName);
                var indexSetup = new MongoDbIndex(_database);
                indexSetup.CreateIndexes();
                
            }
            _collection = _database.GetCollection<T>(collectionName);          
        }

        private void ApplyCollectionSchema(string collectionName)
        {
            var schema = GetSchemaForType();
            if (schema != null)
            {
                var validator = new BsonDocument
            {
                { "$jsonSchema", schema }
            };

                var validationOptions = new BsonDocument
            {
                { "collMod", collectionName },
                { "validator", validator },
                { "validationLevel", "moderate" },
                { "validationAction", "error" }
            };

                _database.RunCommand<BsonDocument>(validationOptions);
            }
        }

        private BsonDocument GetSchemaForType()
        {
            if (typeof(T) == typeof(OrderSubmission))
            {
                return MongoDbSchema.OrderSubmissionSchema;
            }
            else if (typeof(T) == typeof(Priority))
            {
                return MongoDbSchema.PrioritySchema;
            }
            else if (typeof(T) == typeof(Service))
            {
                return MongoDbSchema.ServiceSchema;
            }
            else if (typeof(T) == typeof(Status))
            {
                return MongoDbSchema.StatusSchema;
            }
            else if (typeof(T) == typeof(User))
            {
                return MongoDbSchema.UserSchema;
            }
            else
            {
                return null;
            }
        }

        //private void CreateIndexesIfNecessary(string collectionName)
        //{
        //    var indexSetup = new MongoDbIndex(_database);
        //    indexSetup.CreateIndexes(collectionName);
        //}

        public async Task<List<T>> FindWithProxies(FilterDefinition<T> filter)
        {
            var aggregation = _collection.Aggregate<T>()
                .Match(filter);

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ProxyAttribute>();
                if (attribute != null)
                {
                    var lookup = new BsonDocument
                    {
                        {
                            "$lookup", new BsonDocument
                            {
                                { "from", attribute.PluralName },
                                { "localField", $"{attribute.ForeignKey}" },
                                { "foreignField", "_id" },
                                { "as", property.Name.ToLower() }
                            }
                        }
                    };

                    var unwind = new BsonDocument
                    {
                        {
                            "$unwind", new BsonDocument
                            {
                                { "path", $"${property.Name.ToLower()}" },
                                { "preserveNullAndEmptyArrays", true }
                            }
                        }
                    };

                    aggregation = aggregation.AppendStage<T>(lookup).AppendStage<T>(unwind);
                }
            }
            var list = await aggregation.ToListAsync();
            return list;
        }

        public async Task<List<ObjectId>> SeedDatabase(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public async Task<bool> Any()
        {
            return await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty) > 0;
        }

        public async Task InsertOneAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<T> FindByUsernameAsync(string username)
        {
            var result = await FindWithProxies(Builders<T>.Filter.Eq("name", username));
            return result.FirstOrDefault();
        }

        public async Task<T> FindByIdAsync(ObjectId id)
        {
            var result = await FindWithProxies(Builders<T>.Filter.Eq("_id", id));
            return result.FirstOrDefault();
        }

        public async Task ReplaceOneAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        public async Task DeleteOneAsync(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
