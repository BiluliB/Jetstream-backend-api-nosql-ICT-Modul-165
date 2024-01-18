using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    public class UserLoginDTO
    {
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
