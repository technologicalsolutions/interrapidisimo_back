using System.ComponentModel.DataAnnotations;

namespace Inter.DAL.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Cada materia equivale a 3 créditos
        /// </summary>
        public int Credits { get; set; } = 3; 
        
        [Required]
        public string TeacherId { get; set; } = string.Empty;

        public virtual ApplicationUser Teacher { get; set; } = null!;
        
        public virtual ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
    }
}
