using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    public class UserUnlockDTO
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
    }
}
