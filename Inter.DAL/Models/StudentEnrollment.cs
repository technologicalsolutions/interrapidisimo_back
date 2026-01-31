using System.ComponentModel.DataAnnotations;

namespace Inter.DAL.Models
{
    public class StudentEnrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;   

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public virtual ApplicationUser Student { get; set; } = null!;
    }
}
