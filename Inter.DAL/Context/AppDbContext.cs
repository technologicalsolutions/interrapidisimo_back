using Inter.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inter.DAL.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentEnrollment> StudentEnrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentEnrollment>()
             .HasOne(e => e.Student)
             .WithMany(u => u.Enrollments)
             .HasForeignKey(e => e.StudentId)
             .OnDelete(DeleteBehavior.Restrict);  // ← IMPORTANTE

            builder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.CoursesTeaching)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
                .HasIndex(c => c.Code)
                .IsUnique();

            builder.Entity<StudentEnrollment>()
                .HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique();

            RolesDefault(builder);
        }

        private void RolesDefault(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Administrador",
                    NormalizedName = "ADMINISTRADOR"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Profesor",
                    NormalizedName = "PROFESOR"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Estudiante",
                    NormalizedName = "ESTUDIANTE"
                }
            );
        }
    }
}
