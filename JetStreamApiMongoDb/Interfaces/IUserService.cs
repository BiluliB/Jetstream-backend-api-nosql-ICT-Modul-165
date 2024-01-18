using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.Models;

namespace JetStreamApiMongoDb.Interfaces
{
    /// <summary>
    /// Interface defining the contract for the user service
    /// </summary>
    public interface IUserService
    {
        Task CreateUserAsync(string username, string password, Roles role);
        Task<bool> AuthenticateAsync(string username, string password);
        Task UnlockUserAsync(string username);
        Task<User> GetUserByUsernameAsync(string userName);
    }
}