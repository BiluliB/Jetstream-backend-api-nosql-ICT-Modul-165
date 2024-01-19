using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    public class UserDTO : User
    {
        public new string? Id { get; set; }
    }
}
