using Microsoft.EntityFrameworkCore;
using Checkpoint_14.Models;

namespace Checkpoint_14.Data
{
    public class DBContext
    {
        public class Checkpoint_14DBContext : DbContext
        {
            public DbSet<Teacher> Teachers { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<StudentCourse> StudentCourses { get; set; }

            public Checkpoint_14DBContext(DbContextOptions<Checkpoint_14DBContext> options)
                : base(options)
            {
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<StudentCourse>()
                    .HasKey(sc => new { sc.StudentId, sc.CourseId });

                modelBuilder.Entity<StudentCourse>()
                    .HasOne(sc => sc.Student)
                    .WithMany(s => s.StudentCourses)
                    .HasForeignKey(sc => sc.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<StudentCourse>()
                    .HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentCourses)
                    .HasForeignKey(sc => sc.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Course>()
                    .HasOne(sc => sc.Teacher)
                    .WithMany(c => c.Courses)
                    .HasForeignKey(sc=> sc.TeacherId)
                    .OnDelete(DeleteBehavior.SetNull);
            }


        }
    }
}
