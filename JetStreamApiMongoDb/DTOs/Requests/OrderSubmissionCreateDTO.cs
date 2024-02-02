using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    /// <summary>
    /// DTO for creating a new order submission
    /// </summary>
    public class OrderSubmissionCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string? Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Lastname { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{15}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required]
        public string? PriorityId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public string? ServiceId { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
