using System.ComponentModel.DataAnnotations;

namespace Proj.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Home address is required.")]
        public string Address { get; set; }

        public string InstagramUsername { get; set; }

        [Required]
        public string ValidIdPhotoPath { get; set; }

        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Reservation> Reservations { get; set; }
    }
   }
