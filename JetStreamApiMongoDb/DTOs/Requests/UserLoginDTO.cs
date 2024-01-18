using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    public class UserLoginDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
