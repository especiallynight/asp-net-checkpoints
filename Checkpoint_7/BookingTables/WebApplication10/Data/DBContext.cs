using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication10.Models;

namespace WebApplication10.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public DbSet<TableForBooking> Tables { get; set; }

        public DbSet<Booking> Bookings { get; set; }

    }
}
