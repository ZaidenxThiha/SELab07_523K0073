using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data.Models;

namespace SchoolAppCore.Data;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments => Set<Department>();

    public virtual DbSet<Instructor> Instructors => Set<Instructor>();

    public virtual DbSet<Course> Courses => Set<Course>();

    public virtual DbSet<Student> Students => Set<Student>();

    public virtual DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Departments");
            entity.HasKey(e => e.DepartmentId);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Budget).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("date");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.ToTable("Instructors");
            entity.HasKey(e => e.InstructorId);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.HireDate).HasColumnType("date");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(e => e.CourseId);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.HasOne(d => d.Department)
                .WithMany(p => p.Courses)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(e => e.StudentId);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.EnrollmentDate).HasColumnType("date");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollments");
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.Grade).HasColumnType("decimal(3, 2)");

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
