using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    public class UserUnlockDTO
    {
        [Required]
        [MaxLength(50)]
        [AllowNull, NotNull]
        public string UserName { get; set; }
    }
}
