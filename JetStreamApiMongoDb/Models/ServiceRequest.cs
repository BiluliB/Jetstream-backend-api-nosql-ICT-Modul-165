using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a service request
    /// </summary>
    public class ServiceRequest : BaseModel
    {

        public new List<(string, string)> foreignKeys = new()
        {
            ("priorities", "priority"),
            ("services", "service"),
            ("statuses", "status"),
            ("users", "user")
        };        

        [BsonElement("firstname")]
        [MaxLength(50)]
        public required string Firstname { get; set; }

        [BsonElement("lastname")]
        [MaxLength(50)]
        public required string Lastname { get; set; }

        [BsonElement("email")]
        [EmailAddress]
        public required string Email { get; set; }

        [BsonElement("phone")]
        [Phone]
        public required string Phone { get; set; }

        [BsonElement("priority_id")]
        public required string PriorityId { get; set; }

        [BsonElement("priority")]
        public virtual Priority Priority { get; set; }

        public bool ShouldSerializePriority() => false;

        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; }

        [BsonElement("pickup_date")]
        public DateTime PickupDate { get; set; }

        [BsonElement("service_id")]
        public required string ServiceId { get; set; }

        [BsonElement("service")]
        public virtual Service Service { get; set; }

        public bool ShouldSerializeService() => false;

        [BsonElement("total_price_chf")]
        public decimal TotalPrice_CHF { get; set; }

        [BsonElement("status_id")]
        public required string StatusId { get; set; }

        [BsonElement("status")]
        public virtual Status Status { get; set; }

        public bool ShouldSerializeStatus() => false;

        [BsonElement("comment")]
        [StringLength(500)]
        public string? Comment { get; set; }

        [BsonElement("user_id")]
        public string? UserId { get; set; }

        [BsonElement("user")]
        public virtual User? User { get; set; }

        public bool ShouldSerializeUser() => false;
    }
}
