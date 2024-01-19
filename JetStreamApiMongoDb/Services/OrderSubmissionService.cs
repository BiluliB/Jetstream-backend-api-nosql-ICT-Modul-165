using AutoMapper;
using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace JetStreamApiMongoDb.Services
{
    public class OrderSubmissionService : IOrderSubmissionService
    {
        private readonly IMongoDbContext _context;
        private readonly IMapper _mapper;

        public OrderSubmissionService(IMongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderSubmissionDTO> Create(OrderSubmissionCreateDTO createDTO)
        {
            var offenStatusList = await _context.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Name, "Offen"));
            var offenStatus = offenStatusList.FirstOrDefault();

            if (offenStatus == null)
            {
                throw new InvalidOperationException("Status 'offen' not found in database.");
            }

            var priorityId = ObjectId.Parse(createDTO.PriorityId);
            var serviceId = ObjectId.Parse(createDTO.ServiceId);

            var priorityList = await _context.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq(p => p.Id, priorityId));
            var priority = priorityList.FirstOrDefault();

            var serviceList = await _context.Services.FindWithProxies(Builders<Service>.Filter.Eq(s => s.Id, serviceId));
            var service = serviceList.FirstOrDefault();

            var totalPrice = (priority?.Price ?? 0) + (service?.Price ?? 0);
            var orderSubmission = _mapper.Map<OrderSubmission>(createDTO);
            orderSubmission.Priority = priority;
            orderSubmission.Service = service;
            orderSubmission.Status = offenStatus;
            orderSubmission.StatusId = offenStatus.Id;
            orderSubmission.TotalPrice_CHF = totalPrice;

            await _context.OrderSubmissions.InsertOneAsync(orderSubmission);

            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task<List<OrderSubmissionDTO>> GetAll()
        {
            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(FilterDefinition<OrderSubmission>.Empty);
            return _mapper.Map<List<OrderSubmissionDTO>>(orderSubmissions);
        }

        public async Task<OrderSubmissionDTO> GetById(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq(os => os.Id, id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();
            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task<OrderSubmissionDTO> Update(ObjectId id, OrderSubmissionUpdateDTO updateDTO)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq(os => os.Id, id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();

            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            _mapper.Map(updateDTO, orderSubmission);

            if (updateDTO.PriorityId != null || updateDTO.ServiceId != null || updateDTO.StatusId != null)
            {
                if (updateDTO.PriorityId != null)
                {
                    var priorityId = ObjectId.Parse(updateDTO.PriorityId);
                    var priorityList = await _context.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq(p => p.Id, priorityId));
                    var priority = priorityList.FirstOrDefault();
                    orderSubmission.Priority = priority;
                }

                if (updateDTO.ServiceId != null)
                {
                    var serviceId = ObjectId.Parse(updateDTO.ServiceId);
                    var serviceList = await _context.Services.FindWithProxies(Builders<Service>.Filter.Eq(s => s.Id, serviceId));
                    var service = serviceList.FirstOrDefault();
                    orderSubmission.Service = service;
                }

                if (updateDTO.StatusId != null)
                {
                    var statusId = ObjectId.Parse(updateDTO.StatusId);
                    var statusList = await _context.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Id, statusId));
                    var status = statusList.FirstOrDefault();
                    orderSubmission.Status = status;
                }

                var totalPrice = (orderSubmission.Priority?.Price ?? 0) + (orderSubmission.Service?.Price ?? 0);
                orderSubmission.TotalPrice_CHF = totalPrice;
            }

            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);

            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task<OrderSubmissionDTO> Cancel(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq(os => os.Id, id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();

            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            var storniertStatusList = await _context.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Name, "Storniert"));
            var storniertStatus = storniertStatusList.FirstOrDefault();

            if (storniertStatus == null)
            {
                throw new InvalidOperationException("Status 'Storniert' not found in database.");
            }

            orderSubmission.StatusId = storniertStatus.Id;

            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);

            // Reload the orderSubmission from the database
            orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq(os => os.Id, id));
            orderSubmission = orderSubmissionList.FirstOrDefault();

            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }



        public async Task Delete(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq(os => os.Id, id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();

            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            await _context.OrderSubmissions.DeleteOneAsync(id);
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
