using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private static readonly List<Package> _packages = new()
        {
            new Package
            {
                Id = 1,
                TrackingNumber = "TRK1001",
                RecipientName = "John Doe",
                UnitNumber = "Apt 4B",
                ContactNumber = "09123456789",
                CourierCompany = "FedEx",
                PackageType = "Box",
                ArrivalDateTime = DateTime.Now.AddHours(-5),
                Status = PackageStatus.WaitingForPickup
            },
            new Package
            {
                Id = 2,
                TrackingNumber = "TRK908924824",
                RecipientName = "laurence",
                UnitNumber = "APT450",
                ContactNumber = "09987654321",
                CourierCompany = "FedEx",
                PackageType = "Envelope",
                ArrivalDateTime = DateTime.Now.AddDays(-1),
                Status = PackageStatus.WaitingForPickup
            }
        };

        public IEnumerable<Package> GetAll() => _packages;

        public Package? GetById(int id) => _packages.FirstOrDefault(p => p.Id == id);

        public (IEnumerable<Package> Items, int TotalCount) SearchPaged(
            string? searchTerm,
            PackageStatus? statusFilter,
            string? sortBy = null,
            bool isAscending = true,
            int page = 1,
            int pageSize = 10)
        {
            var query = _packages.AsQueryable();

            if (statusFilter.HasValue)
            {
                query = query.Where(p => p.Status == statusFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(p =>
                    p.TrackingNumber.ToLower().Contains(term) ||
                    p.RecipientName.ToLower().Contains(term) ||
                    p.CourierCompany.ToLower().Contains(term) ||
                    p.UnitNumber.ToLower().Contains(term) ||
                    (p.ContactNumber != null && p.ContactNumber.ToLower().Contains(term)));
            }

            query = sortBy switch
            {
                "tracking" => isAscending ? query.OrderBy(p => p.TrackingNumber) : query.OrderByDescending(p => p.TrackingNumber),
                "recipient" => isAscending ? query.OrderBy(p => p.RecipientName) : query.OrderByDescending(p => p.RecipientName),
                "courier" => isAscending ? query.OrderBy(p => p.CourierCompany) : query.OrderByDescending(p => p.CourierCompany),
                "status" => isAscending ? query.OrderBy(p => p.Status) : query.OrderByDescending(p => p.Status),
                _ => isAscending ? query.OrderBy(p => p.ArrivalDateTime) : query.OrderByDescending(p => p.ArrivalDateTime),
            };

            int totalCount = query.Count();
            var pagedItems = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return (pagedItems, totalCount);
        }

        public void Add(Package package)
        {
            package.Id = _packages.Any() ? _packages.Max(p => p.Id) + 1 : 1;
            _packages.Add(package);
        }

        public void Update(Package package)
        {
            var existing = GetById(package.Id);
            if (existing != null)
            {
                existing.TrackingNumber = package.TrackingNumber;
                existing.RecipientName = package.RecipientName;
                existing.UnitNumber = package.UnitNumber;
                existing.ContactNumber = package.ContactNumber;
                existing.CourierCompany = package.CourierCompany;
                existing.PackageType = package.PackageType;
                existing.ArrivalDateTime = package.ArrivalDateTime;
            }
        }

        public void MarkAsClaimed(int id)
        {
            var package = GetById(id);
            if (package != null)
            {
                package.Status = PackageStatus.Claimed;
                package.ClaimedDateTime = DateTime.Now;
            }
        }

        public void Delete(int id)
        {
            var package = GetById(id);
            if (package != null)
            {
                _packages.Remove(package);
            }
        }
    }
}