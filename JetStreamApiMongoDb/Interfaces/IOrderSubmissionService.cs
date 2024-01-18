using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Interfaces
{
    public interface IOrderSubmissionService
    {
        Task<OrderSubmissionDTO> Create(OrderSubmissionCreateDTO orderSubmissionCreateDTO);
        Task Delete(ObjectId id);
        Task<List<OrderSubmissionDTO>> GetAll();
        Task<OrderSubmissionDTO> GetById(ObjectId id);
        Task<OrderSubmissionDTO> Update(ObjectId id, OrderSubmissionUpdateDTO orderSubmissionUpdateDTO);
    }
}