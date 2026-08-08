using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PackageController : Controller
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IAuditRepository _auditRepository;

        public PackageController(IPackageRepository packageRepository, IAuditRepository auditRepository)
        {
            _packageRepository = packageRepository;
            _auditRepository = auditRepository;
        }

        public IActionResult Index(
            string? searchTerm,
            PackageStatus? statusFilter,
            string? sortBy = "arrival",
            bool isAscending = false,
            int page = 1,
            int pageSize = 10)
        {
            var allPackages = _packageRepository.GetAll().ToList();
            var (pagedItems, totalFilteredCount) = _packageRepository.SearchPaged(
                searchTerm, statusFilter, sortBy, isAscending, page, pageSize);

            int totalPages = (int)Math.Ceiling((double)totalFilteredCount / pageSize);

            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.SortBy = sortBy;
            ViewBag.IsAscending = isAscending;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages < 1 ? 1 : totalPages;
            ViewBag.TotalFilteredCount = totalFilteredCount;

            ViewBag.TotalCount = allPackages.Count;
            ViewBag.WaitingCount = allPackages.Count(p => p.Status == PackageStatus.WaitingForPickup);
            ViewBag.ClaimedTodayCount = allPackages.Count(p =>
                p.Status == PackageStatus.Claimed &&
                p.ClaimedDateTime.HasValue &&
                p.ClaimedDateTime.Value.Date == DateTime.Today);
            ViewBag.OverdueCount = allPackages.Count(p =>
                p.Status == PackageStatus.WaitingForPickup &&
                (DateTime.Now - p.ArrivalDateTime).TotalDays >= 3);

            return View(pagedItems);
        }

        public IActionResult AuditLogs()
        {
            var logs = _auditRepository.GetAll();
            return View(logs);
        }

        public IActionResult Details(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package == null) return NotFound();

            return View(package);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Package());
        }

        [HttpPost]
        public IActionResult Create(Package package)
        {
            if (!ModelState.IsValid) return View(package);

            _packageRepository.Add(package);
            _auditRepository.Log(User.Identity?.Name ?? "Admin", "Registered Package", $"Added package {package.TrackingNumber} for {package.RecipientName}.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package == null) return NotFound();

            return View(package);
        }

        [HttpPost]
        public IActionResult Edit(Package package)
        {
            if (!ModelState.IsValid) return View(package);

            _packageRepository.Update(package);
            _auditRepository.Log(User.Identity?.Name ?? "Admin", "Updated Package", $"Updated package {package.TrackingNumber}.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ConfirmClaim(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package != null)
            {
                _packageRepository.MarkAsClaimed(id);
                _auditRepository.Log(User.Identity?.Name ?? "Admin", "Claimed Package", $"Marked package {package.TrackingNumber} as claimed.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package != null)
            {
                _packageRepository.Delete(id);
                _auditRepository.Log(User.Identity?.Name ?? "Admin", "Deleted Package", $"Deleted package record {package.TrackingNumber}.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}