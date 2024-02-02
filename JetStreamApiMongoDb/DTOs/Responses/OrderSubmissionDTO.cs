using JetStreamApiMongoDb.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    /// <summary>
    /// DTO for order submission
    /// </summary>
    public class OrderSubmissionDTO
    {
        [AllowNull, NotNull]
        public string Id { get; set; }

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
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?[0-9 ]{13}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required]
        public string? PriorityId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        [AllowNull, NotNull]
        public PriorityDTO Priority { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public string? ServiceId { get; set; }

        [AllowNull, NotNull]
        public ServiceDTO Service { get; set; }

        [Required]
        public decimal TotalPrice_CHF { get; set; }

        public string? StatusId { get; set; }

        [AllowNull, NotNull]
        public StatusDTO Status { get; set; }

        [MaxLength(500)]

        [AllowNull, NotNull]
        public string Comment { get; set; }

        public string? UserId { get; set; }

        [AllowNull, NotNull]
        public UserDTO User { get; set; }
    }
}
