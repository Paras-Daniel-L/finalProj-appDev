using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Proj.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public string InstagramUsername { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pickup Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Return Date is required")]
        public DateTime EndDate { get; set; }

        [Required]
        public string PurposeOfRenting { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string DropOffLocation { get; set; }

        [Required(ErrorMessage = "Please upload your valid IDs")]
        public IFormFile ValidIdImage { get; set; }

        [Required(ErrorMessage = "Please upload the proof of payment")]
        public IFormFile ProofOfPaymentImage { get; set; }
        public int CameraId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}