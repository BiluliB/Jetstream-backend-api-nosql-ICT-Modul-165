using JetStreamApiMongoDb.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.DTOs.Responses
{
    public class OrderSubmissionDTO
    {
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
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required]
        public string? PriorityId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriorityDTO Priority { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public string? ServiceId { get; set; }
        public ServiceDTO Service { get; set; }

        [Required]
        public decimal TotalPrice_CHF { get; set; }

        public string? StatusId { get; set; }
        public StatusDTO Status { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
