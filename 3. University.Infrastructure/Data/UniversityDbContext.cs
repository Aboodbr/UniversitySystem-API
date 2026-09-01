using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Infrastructure.Identity;

namespace University.Infrastructure.Data;

public class UniversityDbContext : IdentityDbContext<ApplicationUser>
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<CourseOffering> CourseOfferings { get; set; }
    public DbSet<CourseSchedule> CourseSchedules { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Soft Delete Query Filters
        builder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<CourseOffering>().HasQueryFilter(e => !e.IsDeleted);
    }
}