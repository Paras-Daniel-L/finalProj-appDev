using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proj.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]

        //User
        public int UserId { get; set; }
        public User User { get; set; }

        //Camera
        public int CameraId { get; set; }
        [ForeignKey("CameraId")]
        public virtual Camera Camera { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        public string PurposeOfRenting { get; set; }
        public string Destination { get; set; }
        public string DropOffLocation { get; set; }

        [Required]
        public string Status { get; set; }= "Pending";

    }
}
