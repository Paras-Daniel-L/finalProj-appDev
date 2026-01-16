using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Proj.Models
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string FullName { get; set; }

        public string? Address { get; set; }
        public string? InstagramUsername { get; set; }
        public string? ValidIdPhotoPath { get; set; }

        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Reservation> Reservations { get; set; }
    }
}