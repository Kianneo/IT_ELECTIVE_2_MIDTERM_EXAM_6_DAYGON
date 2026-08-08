using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        void Add(User user);
        bool ValidateCredentials(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, FirstName = "Admin", LastName = "User", Email = "admin@example.com", Username = "admin", Password = "password123" }
        };

        public User? GetByUsername(string username) =>
            _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public void Add(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
        }

        public bool ValidateCredentials(string username, string password) =>
            _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
    }
}