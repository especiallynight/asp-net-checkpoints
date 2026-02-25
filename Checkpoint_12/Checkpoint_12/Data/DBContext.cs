using Checkpoint_12.Models;
using Microsoft.EntityFrameworkCore;

public class Checkpoint_12DBContext : DbContext  
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> Profiles { get; set; }

    public Checkpoint_12DBContext(DbContextOptions<Checkpoint_12DBContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}