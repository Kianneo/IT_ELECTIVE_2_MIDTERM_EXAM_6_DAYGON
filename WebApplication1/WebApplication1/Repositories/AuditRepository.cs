using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private static readonly List<AuditLog> _logs = new();

        public IEnumerable<AuditLog> GetAll() => _logs.OrderByDescending(l => l.Timestamp);

        public void Log(string user, string action, string details)
        {
            _logs.Add(new AuditLog
            {
                Id = _logs.Any() ? _logs.Max(l => l.Id) + 1 : 1,
                User = user,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            });
        }
    }
}