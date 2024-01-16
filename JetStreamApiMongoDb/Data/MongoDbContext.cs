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

            SeedDatabase().Wait();
        }
        public CollectionWrapper<ServiceRequest> ServiceRequests => new(_mapper, _database, "service_requests");
        public CollectionWrapper<User> Users => new(_mapper, _database, "users");
        public CollectionWrapper<Service> Services => new(_mapper, _database, "services");
        public CollectionWrapper<Status> Statuses => new(_mapper, _database, "statuses");
        public CollectionWrapper<Priority> Priorities => new(_mapper, _database, "priorities");

        public async Task SeedDatabase()
        {
            if (!await Priorities.Any())
            {
                var priorities = await Priorities.SeedDatabase(new List<Priority>
                {
                    new () { Name = "Tief", Price = 0 },
                    new () { Name = "Standard", Price = 5 },
                    new () { Name = "Hoch", Price = 10 }
                });
            }

            if (!await Services.Any())
            { 
            var services = await Services.SeedDatabase(new List<Service>
            {
                    new () { Name = "Kleiner Service", Price = 49 },
                    new () { Name = "Grosser Service", Price = 69 },
                    new () { Name = "Rennskiservice", Price = 99 },
                    new () { Name = "Bindung montieren und einstellen", Price = 39 },
                    new () { Name = "Fell zuschneiden", Price = 25 },
                    new () { Name = "Heißwachsen", Price = 18 }
                });
            }

            if (!await Statuses.Any())
            { 
            var statuses = await Statuses.SeedDatabase(new List<Status>
            {
                    new () { Name = "Offen" },
                    new () { Name = "In Arbeit" },
                    new () { Name = "Abgeschlossen" },
                    new () { Name = "Storniert" }
                });
            }
        }
    }




}
