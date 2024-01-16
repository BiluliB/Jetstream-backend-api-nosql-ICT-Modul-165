using JetStreamApiMongoDb.Data;
using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.Interfaces
{
    public interface IMongoDbContext
    {
        CollectionWrapper<Priority> Priorities { get; }
        CollectionWrapper<ServiceRequest> ServiceRequests { get; }
        CollectionWrapper<Service> Services { get; }
        CollectionWrapper<Status> Statuses { get; }
        CollectionWrapper<User> Users { get; }

        Task SeedDatabase();
    }
}