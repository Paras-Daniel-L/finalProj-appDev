using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Proj.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }

        public int CameraId { get; set; }
        public Camera? Camera { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string PurposeOfRenting { get; set; }
        public string Destination { get; set; }
        public string DropOffLocation { get; set; }

        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }

        [NotMapped]
        public IFormFile? ValidIdImage { get; set; }

        [NotMapped]
        public IFormFile? ProofOfPaymentImage { get; set; }

        public string? ValidIdPath { get; set; }
        public string? ProofOfPaymentPath { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}
