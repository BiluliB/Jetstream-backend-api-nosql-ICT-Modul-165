using JetStreamApiMongoDb.Common;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(50)]
        public Roles Role { get; set; }
    }
}
