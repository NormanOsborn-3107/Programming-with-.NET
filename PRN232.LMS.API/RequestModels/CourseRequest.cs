using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.RequestModels
{
    public class CreateCourseRequest
    {
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        public int SemesterId { get; set; }
    }

    public class UpdateCourseRequest
    {
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        public int SemesterId { get; set; }
    }
}
