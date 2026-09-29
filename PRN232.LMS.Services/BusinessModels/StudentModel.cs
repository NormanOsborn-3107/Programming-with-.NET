using System.Text.Json.Serialization;

namespace PRN232.LMS.Services.BusinessModels
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public int TotalEnrollments { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<EnrollmentModel>? Enrollments { get; set; }
    }
}
