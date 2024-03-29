﻿using JetStreamApiMongoDb.Data;
using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.Interfaces
{
    /// <summary>
    /// Interface for MongoDb context
    /// </summary>
    public interface IMongoDbContext
    {
        CollectionWrapper<Priority> Priorities { get; }
        CollectionWrapper<OrderSubmission> OrderSubmissions { get; }
        CollectionWrapper<Service> Services { get; }
        CollectionWrapper<Status> Statuses { get; }
        CollectionWrapper<User> Users { get; }
    }
}