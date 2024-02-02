using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Requests
{
    /// <summary>
    /// DTO for updating an order submission
    /// </summary>
    public class OrderSubmissionUpdateDTO
    {
        [MaxLength(50)]
        public string? Firstname { get; set; }

        [MaxLength(50)]
        public string? Lastname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        public string? PriorityId { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? PickupDate { get; set; }

        public string? ServiceId { get; set; }

        public decimal? TotalPrice_CHF { get; set; }

        public string? StatusId { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
