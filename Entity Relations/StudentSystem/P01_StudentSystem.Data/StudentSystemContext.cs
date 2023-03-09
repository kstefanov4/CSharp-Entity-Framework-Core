using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }
        public StudentSystemContext(DbContextOptions dbContextOptions)
            :base(dbContextOptions)
        {
            
        }
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Homework> Homeworks { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(LocalDb)\\LocalDBDemo;Database=StudentSystem;Integrated Security=True;MultipleActiveResultSets=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            base.OnModelCreating(modelBuilder);
        }

    }
}