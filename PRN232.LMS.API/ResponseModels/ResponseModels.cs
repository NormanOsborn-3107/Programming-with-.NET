namespace PRN232.LMS.API.ResponseModels
{
    public class SemesterResponse
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCourses { get; set; }
    }

    public class CourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int TotalEnrollments { get; set; }
    }

    public class SubjectResponse
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credit { get; set; }
    }

    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public int TotalEnrollments { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<EnrollmentResponse>? Enrollments { get; set; }
    }

    public class EnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime EnrollDate { get; set; }
        public string Status { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public StudentResponse? Student { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public CourseResponse? Course { get; set; }
    }
}
