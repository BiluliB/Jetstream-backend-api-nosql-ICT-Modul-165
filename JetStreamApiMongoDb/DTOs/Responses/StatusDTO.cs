using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    public class StatusDTO : Status
    {
        public new string? Id { get; set; }
    }
}
