using JetStreamApiMongoDb.Common;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    /// <summary>
    /// DTO for creating a new user
    /// </summary>
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }

        [Required]
        public Roles Role { get; set; }
    }
}
