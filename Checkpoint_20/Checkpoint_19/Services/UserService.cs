using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Checkpoint_19.Services
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public interface IUserService
    {
        bool Register(string username, string password);
        bool ValidateUser(string username, string password);
        User? GetUser(string username);
        bool UserExists(string username);
    }

    public class UserService : IUserService
    {
        private static readonly ConcurrentDictionary<string, User> _users = new();
        public bool Register(string username, string password)
        {
            if (_users.ContainsKey(username))
                return false;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _users[username] = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            if (!_users.TryGetValue(username, out var user))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public User? GetUser(string username)
        {
            _users.TryGetValue(username, out var user);
            return user;
        }
        public bool UserExists(string username)
        {
            return _users.ContainsKey(username);
        }
    }
}