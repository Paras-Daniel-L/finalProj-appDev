using Microsoft.EntityFrameworkCore;
using Proj.Models;
using System.Collections.Generic;

namespace Proj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>().HasData(
                new Camera
                {
                    Id = 1,
                    Name = "CANON IXUS",
                    Brand = "Canon",
                    Model = "IXUS",
                    Price = 500,
                    Description = "Compact digital camera",
                    ImageUrl = "canon_ixus.png",
                    IsAvailable = true
                },
                new Camera
                {
                    Id = 2,
                    Name = "CANON EOS",
                    Brand = "Canon",
                    Model = "EOS",
                    Price = 1500,
                    Description = "Professional DSLR camera",
                    ImageUrl = "canon_eos.png",
                    IsAvailable = true
                },
                new Camera
                {
                    Id = 3,
                    Name = "CANON G7X",
                    Brand = "Canon",
                    Model = "G7X",
                    Price = 800,
                    Description = "High-end vlogging camera",
                    ImageUrl = "canon_g7x.png",
                    IsAvailable = true
                }
            );
        }
    }
}
