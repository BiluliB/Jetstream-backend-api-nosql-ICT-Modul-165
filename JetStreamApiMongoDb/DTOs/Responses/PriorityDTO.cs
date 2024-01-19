using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    public class PriorityDTO : Priority
    {
        public new string Id { get; set; }
    }
}
