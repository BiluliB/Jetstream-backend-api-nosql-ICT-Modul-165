using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Interfaces
{
    public interface IOrderSubmissionService
    {
        Task<OrderSubmissionDTO> Create(OrderSubmissionCreateDTO orderSubmissionCreateDTO);
        Task<List<OrderSubmissionDTO>> GetAll();
        Task<OrderSubmissionDTO> GetById(ObjectId id);
        Task<OrderSubmissionDTO> Update(ObjectId id, OrderSubmissionUpdateDTO orderSubmissionUpdateDTO);
        Task<OrderSubmissionDTO> Cancel(ObjectId id);

        Task Delete(ObjectId id);        
    }
}