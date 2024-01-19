using JetStreamApiMongoDb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JetStreamApiMongoDb.Models
{
    /// <summary>
    /// Model for a service request
    /// </summary>
    public class OrderSubmission : BaseModel
    {
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
        public required ObjectId PriorityId { get; set; }

        [BsonElement("priority")]
        [Proxy("priorities", "priority_id")]
        public virtual Priority Priority { get; set; }

        public bool ShouldSerializePriority() => false;

        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; }

        [BsonElement("pickup_date")]
        public DateTime PickupDate { get; set; }

        [BsonElement("service_id")]
        public required ObjectId ServiceId { get; set; }

        [BsonElement("service")]
        [Proxy("services", "service_id")]
        public virtual Service Service { get; set; }

        public bool ShouldSerializeService() => false;

        [BsonElement("total_price_chf")]
        public decimal TotalPrice_CHF { get; set; }

        [BsonElement("status_id")]
        public required ObjectId StatusId { get; set; }

        [BsonElement("status")]
        [Proxy("statuses", "status_id")]
        public virtual Status Status { get; set; }

        public bool ShouldSerializeStatus() => false;

        [BsonElement("comment")]
        [StringLength(500)]
        public string? Comment { get; set; }

        [BsonElement("user_id")]
        public ObjectId? UserId { get; set; } = null;

        [BsonElement("user")]
        [Proxy("users", "user_id")]
        public virtual User? User { get; set; }

        public bool ShouldSerializeUser() => false;
    }
}
