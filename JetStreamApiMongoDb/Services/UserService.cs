using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.Models;
using JetStreamApiMongoDb.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JetStreamApiMongoDb.Services
{
    public class UserService : IUserService
    {
        private readonly CollectionWrapper<User> _users;

        public UserService(IMongoDbContext mongoDbContext)
        {
            _users = mongoDbContext.Users;
        }

        public async Task CreateUserAsync(string username, string password, Roles role)
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

        public async Task<bool> AuthenticateAsync(string username, string password)
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

        public async Task UnlockUserAsync(string username)
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

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _users.FindByUsernameAsync(userName);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

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
