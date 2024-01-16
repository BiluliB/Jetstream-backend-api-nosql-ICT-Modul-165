using AutoMapper;
using JetStreamApiMongoDb.Models;
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
        public CollectionWrapper(IMapper mapper, IMongoDatabase database, string name)
        {            _database = database;
            _collection = _database.GetCollection<T>(name);
        }
        public async Task<List<T>> FindWithProxies(FilterDefinition<T> filter)
        {
            var aggregation = _collection.Aggregate()
                .Match(filter);
            var field = typeof(T).GetField("foreignKeys", BindingFlags.Static | BindingFlags.Public);
            if ( field == null )                
            {
                field = typeof(T).BaseType.GetField("foreignKeys", BindingFlags.Static | BindingFlags.Public);
            }

            foreach (var (collectionName, foreignKey) in (List<(string, string)>) field.GetValue(null) )
            {
                aggregation = (IAggregateFluent<T>)aggregation
                    .Lookup(collectionName, $"{foreignKey}_id", "_id", foreignKey)
                    .Unwind($"${foreignKey}"); 
            }
            return await aggregation.ToListAsync();
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
    }
}
