using Checkpoint_13.Models;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint_13.Data
{
    public class Checkpoint_13DBContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public Checkpoint_13DBContext(DbContextOptions<Checkpoint_13DBContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
