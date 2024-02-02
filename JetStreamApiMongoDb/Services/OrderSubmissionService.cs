using AutoMapper;
using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;

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

        /// <summary>
        /// Create a new order submission
        /// </summary>
        /// <param name="createDTO"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<OrderSubmissionDTO> Create(OrderSubmissionCreateDTO createDTO)
        {
            var offenStatusList = await _context.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Name, "Offen"));
            var offenStatus = offenStatusList.FirstOrDefault();

            if (offenStatus == null)
            {
                throw new InvalidOperationException("Status 'offen' not found in database.");
            }

            var priorityId = ObjectId.Parse(createDTO.PriorityId);
            var priorityList = await _context.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq(p => p.Id, priorityId));
            var priority = priorityList.FirstOrDefault();

            if (priority == null)
            {
                throw new KeyNotFoundException($"Priority with ID '{createDTO.PriorityId}' not found.");
            }

            var serviceId = ObjectId.Parse(createDTO.ServiceId);
            var serviceList = await _context.Services.FindWithProxies(Builders<Service>.Filter.Eq(s => s.Id, serviceId));
            var service = serviceList.FirstOrDefault();

            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID '{createDTO.ServiceId}' not found.");
            }

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

        /// <summary>
        /// Gets all order submissions
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderSubmissionDTO>> GetAll()
        {
            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(FilterDefinition<OrderSubmission>.Empty);
            return _mapper.Map<List<OrderSubmissionDTO>>(orderSubmissions);
        }

        public async Task<OrderSubmissionDTO> GetById(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();
            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        /// <summary>
        /// Updates an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDTO"></param>
        /// <returns>OrderSubmissionDTO</returns>
        /// <exception cref="InvalidOperationException">If order submission not found</exception>
        public async Task<OrderSubmissionDTO> Update(ObjectId id, OrderSubmissionUpdateDTO updateDTO)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", id));
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
                    var priorityList = await _context.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq("_id", priorityId));
                    var priority = priorityList.FirstOrDefault();
                    orderSubmission.Priority = priority;
                }

                if (updateDTO.ServiceId != null)
                {
                    var serviceId = ObjectId.Parse(updateDTO.ServiceId);
                    var serviceList = await _context.Services.FindWithProxies(Builders<Service>.Filter.Eq("_id", serviceId));
                    var service = serviceList.FirstOrDefault();
                    orderSubmission.Service = service;
                }

                var totalPrice = (orderSubmission.Priority?.Price ?? 0) + (orderSubmission.Service?.Price ?? 0);
                orderSubmission.TotalPrice_CHF = totalPrice;

            }

            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);

            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        /// <summary>
        /// Cancels an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OrderSubmissionDTO</returns>
        /// <exception cref="InvalidOperationException">orderSubmission not found and status 'Storniert' not found</exception>
        public async Task<OrderSubmissionDTO> Cancel(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", id));
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

            orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", id));
            orderSubmission = orderSubmissionList.FirstOrDefault();

            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        /// <summary>
        /// Deletes an order submission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Order submission not found</exception>
        public async Task Delete(ObjectId id)
        {
            var orderSubmissionList = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", id));
            var orderSubmission = orderSubmissionList.FirstOrDefault();

            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            await _context.OrderSubmissions.DeleteOneAsync(id);
        }

        /// <summary>
        /// Assigns an order submission to a user
        /// </summary>
        /// <param name="orderSubmissionId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">User not found and order submission not found</exception>
        public async Task AssignOrderSubmissionToUser(string orderSubmissionId, string userId)
        {
            var orderSubmissionObjectId = new ObjectId(orderSubmissionId);
            var userObjectId = new ObjectId(userId);

            var users = await _context.Users.FindWithProxies(Builders<User>.Filter.Eq("_id", userObjectId));
            var user = users.FirstOrDefault();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", orderSubmissionObjectId));
            var orderSubmission = orderSubmissions.FirstOrDefault();
            if (orderSubmission == null)
            {
                throw new KeyNotFoundException($"OrderSubmission with ID {orderSubmissionId} not found.");
            }

            orderSubmission.UserId = userObjectId;

            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);
        }

        /// <summary>
        /// Updates an order submission
        /// </summary>
        /// <param name="orderSubmissionId"></param>
        /// <param name="newUserId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">user not found and order submission not found</exception>
        public async Task UpdateOrderSubmission(string orderSubmissionId, string newUserId)
        {
            var orderSubmissionObjectId = new ObjectId(orderSubmissionId);
            var newUserObjectId = new ObjectId(newUserId);

            var users = await _context.Users.FindWithProxies(Builders<User>.Filter.Eq("_id", newUserObjectId));
            var user = users.FirstOrDefault();
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {newUserId} not found.");
            }

            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(Builders<OrderSubmission>.Filter.Eq("_id", orderSubmissionObjectId));
            var orderSubmission = orderSubmissions.FirstOrDefault();
            if (orderSubmission == null)
            {
                throw new KeyNotFoundException($"OrderSubmission with ID {orderSubmissionId} not found.");
            }

            orderSubmission.UserId = newUserObjectId;

            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);
        }
    }
}
