using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN232.LMS.Repositories.Entities
{
    [Table("Enrollment")]
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public DateTime EnrollDate { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        // Navigation
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
