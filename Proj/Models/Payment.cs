using System.ComponentModel.DataAnnotations;

namespace Proj.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
        public string ProofOfPaymentPath { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}