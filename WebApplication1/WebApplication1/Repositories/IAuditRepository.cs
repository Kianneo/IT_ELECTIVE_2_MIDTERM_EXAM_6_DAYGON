using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IAuditRepository
    {
        IEnumerable<AuditLog> GetAll();
        void Log(string user, string action, string details);
    }
}