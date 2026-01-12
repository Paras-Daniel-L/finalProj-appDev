using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proj.Models
{
    public class Camera
    {
        [Key]     
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
    }
}
