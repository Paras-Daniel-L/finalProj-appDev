using Microsoft.EntityFrameworkCore;
using Proj.Models;

namespace Proj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
    }
}