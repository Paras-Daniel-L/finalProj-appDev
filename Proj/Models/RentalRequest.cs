using System.ComponentModel.DataAnnotations;

namespace Proj.Models
{
    public class RentalRequest
    {
        [Key]
        public int Id { get; set; }

        // User Details
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string InstagramUsername { get; set; }
        public string Email { get; set; }

        // Rental Details
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Purpose { get; set; }
        public string Destination { get; set; }
        public string DropoffLocation { get; set; }

        // Camera Details (Passed from Product Page)
        public string CameraModel { get; set; }
        public decimal TotalPayment { get; set; }

        // File Paths (For the images uploaded)
        public string ValidIdPath { get; set; }
        public string PaymentProofPath { get; set; }

        // Admin Status
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}