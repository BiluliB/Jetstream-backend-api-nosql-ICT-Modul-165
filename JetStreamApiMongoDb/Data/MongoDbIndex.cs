using MongoDB.Driver;
using JetStreamApiMongoDb.Models;
using System.Threading.Tasks;

namespace JetStreamApiMongoDb.Data
{
    public class MongoDbIndex(IMongoDatabase database)
    {
        private readonly IMongoDatabase _database = database;

        public void CreateIndexes()

        {
            CreateOrderSubmissionIndexes();
            CreatePriorityIndexes();
            CreateServiceIndexes();
            CreateStatusIndexes();
            CreateUserIndexes();
        }

        private void CreateOrderSubmissionIndexes()
        {
            var orderSubmissions = _database.GetCollection<OrderSubmission>("order_submissions");

            // Composite-Index für CreateDate und StatusId
            var compositeIndexKeys = Builders<OrderSubmission>.IndexKeys
                .Ascending(os => os.PickupDate)
                .Ascending(os => os.StatusId);
            var compositeIndexOptions = new CreateIndexOptions { Name = "PickupDate_StatusId_index" };
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(compositeIndexKeys, compositeIndexOptions));

            // Indizes für Fremdschlüsselfelder
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.PriorityId),
                new CreateIndexOptions { Name = "PriorityId_index" }));

            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.ServiceId),
                new CreateIndexOptions { Name = "ServiceId_index" }));

            // Index für Firstname
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.Firstname),
                new CreateIndexOptions { Name = "Firstname_index" }));

            // Index für Lastname
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.Lastname),
                new CreateIndexOptions { Name = "Lastname_index" }));

            // Index für Email
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.Email),
                new CreateIndexOptions { Name = "Email_index" }));

            // Index für Phone
            orderSubmissions.Indexes.CreateOne(new CreateIndexModel<OrderSubmission>(
                Builders<OrderSubmission>.IndexKeys.Ascending(os => os.Phone),
                new CreateIndexOptions { Name = "Phone_index" }));


        }

        private void CreatePriorityIndexes()
        {
            var priorities = _database.GetCollection<Priority>("priorities");

            // Index auf Name in Priority
            priorities.Indexes.CreateOne(new CreateIndexModel<Priority>(
                Builders<Priority>.IndexKeys.Ascending(p => p.Name),
                new CreateIndexOptions { Name = "Priority_name_index" }));

            // Weitere Indizes nach Bedarf...
        }

        private void CreateServiceIndexes()
        {
            var services = _database.GetCollection<Service>("services");

            // Index auf Name in Service
            services.Indexes.CreateOne(new CreateIndexModel<Service>(
                Builders<Service>.IndexKeys.Ascending(s => s.Name),
                new CreateIndexOptions { Name = "Service_name_index" }));

            // Weitere Indizes nach Bedarf...
        }

        private void CreateStatusIndexes()
        {
            var statuses = _database.GetCollection<Status>("statuses");

            // Index auf Name in Status
            statuses.Indexes.CreateOne(new CreateIndexModel<Status>(
                Builders<Status>.IndexKeys.Ascending(st => st.Name),
                new CreateIndexOptions { Name = "Status_name_index" }));

            // Weitere Indizes nach Bedarf...
        }

        private void CreateUserIndexes()
        {
            var users = _database.GetCollection<User>("users");

            // Indizes für Benutzerfelder, z.B. Name, Email usw.
            users.Indexes.CreateOne(new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(u => u.Name),
                new CreateIndexOptions { Name = "User_name_index" }));
        }
    }
}
