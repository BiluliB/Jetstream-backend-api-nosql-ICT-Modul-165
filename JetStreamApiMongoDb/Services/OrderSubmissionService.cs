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

        public async Task<OrderSubmissionDTO> Create(OrderSubmissionCreateDTO createDTO)
        {
            // Find the 'offen' status in the database with proxies
            var offenStatusList = await _context.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Name, "Offen"));
            var offenStatus = offenStatusList.FirstOrDefault();

            if (offenStatus == null)
            {
                throw new InvalidOperationException("Status 'offen' not found in database.");
            }

            // Konvertieren der String-IDs in ObjectId
            var priorityId = ObjectId.Parse(createDTO.PriorityId);
            var serviceId = ObjectId.Parse(createDTO.ServiceId);

            // Laden der vollständigen Priority und Service Objekte
            var priority = await _context.Priorities.FindByIdAsync(priorityId);
            var service = await _context.Services.FindByIdAsync(serviceId);

            // Berechnen des Gesamtpreises und Erstellen der OrderSubmission
            var totalPrice = (priority?.Price ?? 0) + (service?.Price ?? 0);
            var orderSubmission = _mapper.Map<OrderSubmission>(createDTO);
            orderSubmission.Priority = priority;
            orderSubmission.Service = service;
            orderSubmission.Status = offenStatus; // Hier wird das Status-Objekt gesetzt
            orderSubmission.StatusId = offenStatus.Id; // Setzen der StatusId
            orderSubmission.TotalPrice_CHF = totalPrice;

            // Save to database
            await _context.OrderSubmissions.InsertOneAsync(orderSubmission);

            // Da die verknüpften Daten bereits gesetzt sind, kann das DTO direkt gemappt werden
            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task<List<OrderSubmissionDTO>> GetAll()
        {
            var orderSubmissions = await _context.OrderSubmissions.FindWithProxies(FilterDefinition<OrderSubmission>.Empty);
            return _mapper.Map<List<OrderSubmissionDTO>>(orderSubmissions);
        }

        public async Task<OrderSubmissionDTO> GetById(ObjectId id)
        {
            var orderSubmission = await _context.OrderSubmissions.FindByIdAsync(id);
            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task<OrderSubmissionDTO> Update(ObjectId id, OrderSubmissionUpdateDTO updateDTO)
        {
            // Find the existing order submission
            var orderSubmission = await _context.OrderSubmissions.FindByIdAsync(id);
            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            // Update the order submission with the new values
            _mapper.Map(updateDTO, orderSubmission);

            // If the priority, status or service has been updated, load the new objects and recalculate the total price
            if (updateDTO.PriorityId != null || updateDTO.ServiceId != null || updateDTO.StatusId != null)
            {
                if (updateDTO.PriorityId != null)
                {
                    var priorityId = ObjectId.Parse(updateDTO.PriorityId);
                    var priority = await _context.Priorities.FindByIdAsync(priorityId);
                    orderSubmission.Priority = priority;
                }

                if (updateDTO.ServiceId != null)
                {
                    var serviceId = ObjectId.Parse(updateDTO.ServiceId);
                    var service = await _context.Services.FindByIdAsync(serviceId);
                    orderSubmission.Service = service;
                }

                if (updateDTO.StatusId != null)
                {
                    var statusId = ObjectId.Parse(updateDTO.StatusId);
                    var status = await _context.Statuses.FindByIdAsync(statusId);
                    orderSubmission.Status = status;
                }

                var totalPrice = (orderSubmission.Priority?.Price ?? 0) + (orderSubmission.Service?.Price ?? 0);
                orderSubmission.TotalPrice_CHF = totalPrice;
            }

            // Save the updated order submission back to the database
            await _context.OrderSubmissions.ReplaceOneAsync(orderSubmission);

            // Return the updated order submission
            return _mapper.Map<OrderSubmissionDTO>(orderSubmission);
        }

        public async Task Delete(ObjectId id)
        {
            // Find the existing order submission
            var orderSubmission = await _context.OrderSubmissions.FindByIdAsync(id);
            if (orderSubmission == null)
            {
                throw new InvalidOperationException("Order submission not found.");
            }

            // Delete the order submission from the database
            await _context.OrderSubmissions.DeleteOneAsync(id);
        }

    }
}
