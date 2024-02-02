using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    /// <summary>
    /// DTO for status
    /// </summary>
    public class StatusDTO : Status
    {
        public new string? Id { get; set; }
    }
}
