using JetStreamApiMongoDb.Models;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    /// <summary>
    /// DTO for priority
    /// </summary>
    public class PriorityDTO : Priority
    {
        [AllowNull, NotNull]
        public new string Id { get; set; }
    }
}
