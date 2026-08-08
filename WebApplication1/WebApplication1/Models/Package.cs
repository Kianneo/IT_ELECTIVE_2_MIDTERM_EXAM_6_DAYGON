using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Package
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tracking Number is required.")]
        [Display(Name = "Tracking Number")]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Recipient Name is required.")]
        [StringLength(100, ErrorMessage = "Recipient Name cannot exceed 100 characters.")]
        [Display(Name = "Recipient Name")]
        public string RecipientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unit/Office number is required.")]
        [Display(Name = "Unit / Office")]
        public string UnitNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Courier Company is required.")]
        [Display(Name = "Courier Company")]
        public string CourierCompany { get; set; } = string.Empty;

        [Required(ErrorMessage = "Package Type is required.")]
        [Display(Name = "Package Type")]
        public string PackageType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arrival Date & Time is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Arrival Date & Time")]
        public DateTime ArrivalDateTime { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Expected Pickup Date")]
        public DateTime? ExpectedPickupDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Claimed Date & Time")]
        public DateTime? ClaimedDateTime { get; set; }

        [Required(ErrorMessage = "Received By is required.")]
        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public PackageStatus Status { get; set; } = PackageStatus.WaitingForPickup;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}