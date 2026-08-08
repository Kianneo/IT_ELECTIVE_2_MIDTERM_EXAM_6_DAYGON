using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAll();
        Package? GetById(int id);
        (IEnumerable<Package> Items, int TotalCount) SearchPaged(
            string? searchTerm,
            PackageStatus? statusFilter,
            string? sortBy = null,
            bool isAscending = true,
            int page = 1,
            int pageSize = 10);
        void Add(Package package);
        void Update(Package package);
        void MarkAsClaimed(int id);
        void Delete(int id);
    }
}