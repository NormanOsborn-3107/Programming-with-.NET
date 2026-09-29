namespace PRN232.LMS.Services.BusinessModels
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public int TotalEnrollments { get; set; }
    }
}
