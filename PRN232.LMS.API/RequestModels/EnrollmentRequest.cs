using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.RequestModels
{
    public class CreateEnrollmentRequest
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime EnrollDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";
    }

    public class UpdateEnrollmentRequest
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime EnrollDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}
