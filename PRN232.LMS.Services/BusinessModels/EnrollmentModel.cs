using System.Text.Json.Serialization;

namespace PRN232.LMS.Services.BusinessModels
{
    public class EnrollmentModel
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime EnrollDate { get; set; }
        public string Status { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public StudentModel? Student { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CourseModel? Course { get; set; }
    }
}
