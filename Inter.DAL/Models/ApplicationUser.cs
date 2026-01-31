using Microsoft.AspNetCore.Identity;

namespace Inter.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();        

        public virtual ICollection<Course> CoursesTeaching { get; set; } = new List<Course>();
    }
}
