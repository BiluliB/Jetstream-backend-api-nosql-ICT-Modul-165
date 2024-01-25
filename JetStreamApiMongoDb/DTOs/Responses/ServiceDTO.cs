using JetStreamApiMongoDb.Models;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    public class ServiceDTO : Service
    {
        [AllowNull, NotNull]
        public new string Id { get; set; }
    }
}
