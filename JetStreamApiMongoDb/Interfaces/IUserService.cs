using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Models;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Interfaces
{
    /// <summary>
    /// Interface defining the contract for the user service
    /// </summary>
    public interface IUserService
    {
        Task CreateUser(string username, string password, Roles role);
        Task<List<UserDTO>> GetAll();
        Task<UserDTO> GetById(string id);
        Task Delete(ObjectId id);
        Task<bool> Authenticate(string username, string password);
        Task UnlockUser(string username);
        Task<User> GetUserByUsername(string userName);
    }
}