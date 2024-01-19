using AutoMapper;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JetStreamApiMongoDb.Services
{
    public class OrderAssignmentService : IOrderAssignmentService
    {
        private readonly IMongoDbContext _context;
        private readonly IMapper _mapper;

        public OrderAssignmentService(IMongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AssignOrderSubmissionToUser(string orderSubmissionId, string userId)
        {
            // Convert string IDs to ObjectId
            var orderSubmissionObjectId = new ObjectId(orderSubmissionId);
            var userObjectId = new ObjectId(userId);

            // Check if the user exists
            var users = await _context.Users.FindWithProxies(Builders<User>.Filter.Eq("_id", userObjectId));
            var user = users.FirstOrDefault();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Find the order submission
            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", orderSubmissionObjectId));
            var orderSubmission = orderSubmissions.FirstOrDefault();
            if (orderSubmission == null)
            {
                throw new KeyNotFoundException($"OrderSubmission with ID {orderSubmissionId} not found.");
            }

            orderSubmission.UserId = userObjectId;

            // Update the order submission
            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);
        }

        public async Task UpdateOrderSubmission(string orderSubmissionId, string newUserId)
        {
            // Convert string IDs to ObjectId
            var orderSubmissionObjectId = new ObjectId(orderSubmissionId);
            var newUserObjectId = new ObjectId(newUserId);

            // Check if the new user exists
            var users = await _context.Users.FindWithProxies(Builders<User>.Filter.Eq("_id", newUserObjectId));
            var user = users.FirstOrDefault();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {newUserId} not found.");
            }

            // Find the order submission
            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", orderSubmissionObjectId));
            var orderSubmission = orderSubmissions.FirstOrDefault();
            if (orderSubmission == null)
            {
                throw new KeyNotFoundException($"OrderSubmission with ID {orderSubmissionId} not found.");
            }

            orderSubmission.UserId = newUserObjectId;

            // Update the order submission
            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);
        }
    }
}
