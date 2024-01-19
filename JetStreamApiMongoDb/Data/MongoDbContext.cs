using AutoMapper;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using MongoDB.Driver;

namespace JetStreamApiMongoDb.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMapper _mapper;

        public MongoDbContext(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;

            var mongoSection = configuration.GetSection("MongoDb");
            var url = mongoSection.GetValue<string>("Url");
            var database = mongoSection.GetValue<string>("Database");

            var client = new MongoClient(url);
            _database = client.GetDatabase(database);

            //SeedDatabase().Wait();
        }
        public CollectionWrapper<OrderSubmission> OrderSubmissions => new(_mapper, _database, "order_submissions");
        public CollectionWrapper<User> Users => new(_mapper, _database, "users");
        public CollectionWrapper<Service> Services => new(_mapper, _database, "services");
        public CollectionWrapper<Status> Statuses => new(_mapper, _database, "statuses");
        public CollectionWrapper<Priority> Priorities => new(_mapper, _database, "priorities");

        public CollectionWrapper<T> Get<T>()
            where T : BaseModel
        {
            var propertyInfo = GetType()
                .GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(CollectionWrapper<T>));
            if (propertyInfo != null)
            {
                return (CollectionWrapper<T>)propertyInfo.GetValue(this)!;
            }
            else
            {
                throw new InvalidOperationException($"No collection wrapper found for type {typeof(T).Name}");
            }
        }
    }
}
