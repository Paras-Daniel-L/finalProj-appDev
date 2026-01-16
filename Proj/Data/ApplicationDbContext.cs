using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proj.Models;

namespace Proj.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Camera>().HasData(
                new Camera
                {
                    Id = 1,
                    Name = "Canon G7X",
                    Brand = "Canon",
                    Model = "G7X",
                    Price = 800,
                    ImageUrl = "canon_g7x.png", // Make sure you have an image with this name!
                    Description = "Compact premium camera perfect for vlogging.",
                    IsAvailable = true
                },
                new Camera
                {
                    Id = 2,
                    Name = "Canon IXUS",
                    Brand = "Canon",
                    Model = "IXUS",
                    Price = 500,
                    ImageUrl = "canon_ixus.png",
                    Description = "Slim, stylish, and easy to use point-and-shoot.",
                    IsAvailable = true
                },
                new Camera
                {
                    Id = 3,
                    Name = "Canon EOS",
                    Brand = "Canon",
                    Model = "EOS",
                    Price = 1500,
                    ImageUrl = "canon_eos.png",
                    Description = "Versatile DSLR for high-quality photography.",
                    IsAvailable = true
                }
            );
        }
    }
}