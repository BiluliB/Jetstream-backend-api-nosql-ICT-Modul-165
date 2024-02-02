using AutoMapper;
using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.Data;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace JetStreamApiMongoDb.Services
{
    public class UserService : IUserService
    {
        private readonly CollectionWrapper<User> _users;
        private readonly IMongoDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IMongoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _users = context.Users;
        }

        /// <summary>
        /// Creates a new user with the given username, password and role.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Ungültige Rolle.</exception>
        public async Task CreateUser(string username, string password, Roles role)
        {
            if (!Enum.IsDefined(typeof(Roles), role))
            {
                throw new ArgumentException("Ungültige Rolle.");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Name = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            await _users.InsertOneAsync(user);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Users</returns>
        public async Task<List<UserDTO>> GetAll()
        {
            var users = await _context.Users.FindWithProxies(FilterDefinition<User>.Empty);
            return _mapper.Map<List<UserDTO>>(users);
        }

        /// <summary>
        /// Gets a user by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user</returns>
        public async Task<UserDTO> GetById(string id)
        {
            var user = await _context.Users.FindWithProxies(Builders<User>.Filter.Eq(u => u.Id, ObjectId.Parse(id)));
            return _mapper.Map<UserDTO>(user.FirstOrDefault());
        }

        /// <summary>
        /// Authenticates a user with the given username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if the user is authenticated, false otherwise</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> Authenticate(string username, string password)
        {
            var user = await _users.FindByUsernameAsync(username);

            if (user == null)
            {
                throw new ArgumentException("Benutzername nicht gefunden.");
            }

            if (user.IsLocked)
            {
                throw new InvalidOperationException("Benutzerkonto ist gesperrt.");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                user.PasswordInputAttempt += 1;
                await _users.ReplaceOneAsync(user);

                if (user.PasswordInputAttempt >= 3)
                {
                    user.IsLocked = true;
                    await _users.ReplaceOneAsync(user);
                    throw new InvalidOperationException("Benutzerkonto wurde wegen zu vieler fehlgeschlagener Versuche gesperrt.");
                }

                int remainingAttempts = 3 - user.PasswordInputAttempt;
                throw new ArgumentException($"Falsches Passwort. Verbleibende Versuche: {remainingAttempts}");
            }

            user.PasswordInputAttempt = 0;
            await _users.ReplaceOneAsync(user);
            return true;
        }

        /// <summary>
        /// Unlocks a user with the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>UserName</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task UnlockUser(string username)
        {
            var user = await _users.FindByUsernameAsync(username);

            if (user == null)
            {
                throw new ArgumentException("Benutzername nicht gefunden.");
            }

            user.IsLocked = false;
            user.PasswordInputAttempt = 0;
            await _users.ReplaceOneAsync(user);
        }

        /// <summary>
        /// Gets a user by its username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>userName</returns>
        public async Task<User> GetUserByUsername(string userName)
        {
            return await _users.FindByUsernameAsync(userName);
        }

        /// <summary>
        /// Deletes a user by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(ObjectId id)
        {
            await _users.DeleteOneAsync(id);
        }

        /// <summary>
        /// Creates a password hash and salt for the given password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies the given password with the given password hash and salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
