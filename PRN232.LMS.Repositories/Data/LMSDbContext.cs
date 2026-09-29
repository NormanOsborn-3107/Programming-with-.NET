using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options) { }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── SEED: 5 Semesters ──────────────────────────────────────────
            modelBuilder.Entity<Semester>().HasData(
                new Semester { SemesterId = 1, SemesterName = "Spring 2023", StartDate = new DateTime(2023, 1, 10), EndDate = new DateTime(2023, 4, 30) },
                new Semester { SemesterId = 2, SemesterName = "Summer 2023", StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 8, 31) },
                new Semester { SemesterId = 3, SemesterName = "Fall 2023",   StartDate = new DateTime(2023, 9, 5),  EndDate = new DateTime(2023, 12, 31) },
                new Semester { SemesterId = 4, SemesterName = "Spring 2024", StartDate = new DateTime(2024, 1, 8),  EndDate = new DateTime(2024, 4, 30) },
                new Semester { SemesterId = 5, SemesterName = "Summer 2024", StartDate = new DateTime(2024, 5, 6),  EndDate = new DateTime(2024, 8, 31) }
            );

            // ── SEED: 10 Subjects ─────────────────────────────────────────
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1,  SubjectCode = "CS101",  SubjectName = "Introduction to Programming",  Credit = 3 },
                new Subject { SubjectId = 2,  SubjectCode = "CS201",  SubjectName = "Data Structures & Algorithms", Credit = 3 },
                new Subject { SubjectId = 3,  SubjectCode = "CS301",  SubjectName = "Database Management",          Credit = 3 },
                new Subject { SubjectId = 4,  SubjectCode = "CS401",  SubjectName = "Software Engineering",         Credit = 3 },
                new Subject { SubjectId = 5,  SubjectCode = "CS501",  SubjectName = "Computer Networks",            Credit = 3 },
                new Subject { SubjectId = 6,  SubjectCode = "MA101",  SubjectName = "Calculus I",                   Credit = 3 },
                new Subject { SubjectId = 7,  SubjectCode = "MA201",  SubjectName = "Linear Algebra",               Credit = 3 },
                new Subject { SubjectId = 8,  SubjectCode = "EN101",  SubjectName = "Technical English",            Credit = 2 },
                new Subject { SubjectId = 9,  SubjectCode = "CS601",  SubjectName = "Web Development",              Credit = 3 },
                new Subject { SubjectId = 10, SubjectCode = "CS701",  SubjectName = "Machine Learning",             Credit = 3 }
            );

            // ── SEED: 20 Courses ──────────────────────────────────────────
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1,  CourseName = "Intro to Programming - Spring 23",  SemesterId = 1 },
                new Course { CourseId = 2,  CourseName = "Data Structures - Spring 23",        SemesterId = 1 },
                new Course { CourseId = 3,  CourseName = "Database Mgmt - Spring 23",          SemesterId = 1 },
                new Course { CourseId = 4,  CourseName = "Software Eng - Spring 23",           SemesterId = 1 },
                new Course { CourseId = 5,  CourseName = "Intro to Programming - Summer 23",   SemesterId = 2 },
                new Course { CourseId = 6,  CourseName = "Web Development - Summer 23",        SemesterId = 2 },
                new Course { CourseId = 7,  CourseName = "Technical English - Summer 23",      SemesterId = 2 },
                new Course { CourseId = 8,  CourseName = "Linear Algebra - Summer 23",         SemesterId = 2 },
                new Course { CourseId = 9,  CourseName = "Computer Networks - Fall 23",        SemesterId = 3 },
                new Course { CourseId = 10, CourseName = "Database Mgmt - Fall 23",            SemesterId = 3 },
                new Course { CourseId = 11, CourseName = "Software Eng - Fall 23",             SemesterId = 3 },
                new Course { CourseId = 12, CourseName = "Machine Learning - Fall 23",         SemesterId = 3 },
                new Course { CourseId = 13, CourseName = "Calculus I - Spring 24",             SemesterId = 4 },
                new Course { CourseId = 14, CourseName = "Data Structures - Spring 24",        SemesterId = 4 },
                new Course { CourseId = 15, CourseName = "Web Development - Spring 24",        SemesterId = 4 },
                new Course { CourseId = 16, CourseName = "Technical English - Spring 24",      SemesterId = 4 },
                new Course { CourseId = 17, CourseName = "Computer Networks - Summer 24",      SemesterId = 5 },
                new Course { CourseId = 18, CourseName = "Machine Learning - Summer 24",       SemesterId = 5 },
                new Course { CourseId = 19, CourseName = "Linear Algebra - Summer 24",         SemesterId = 5 },
                new Course { CourseId = 20, CourseName = "Intro to Programming - Summer 24",   SemesterId = 5 }
            );

            // ── SEED: 50 Students ─────────────────────────────────────────
            var students = new List<Student>();
            var firstNames = new[] { "An", "Bảo", "Chi", "Dũng", "Em", "Phúc", "Giang", "Hùng", "Khánh", "Lan",
                                     "Minh", "Nam", "Oanh", "Phương", "Quân", "Rộng", "Sơn", "Tâm", "Uyên", "Vân",
                                     "Xuân", "Yến", "Ánh", "Bình", "Cường", "Diễm", "Đức", "Gia", "Hoa", "Khoa",
                                     "Linh", "Long", "Mai", "Ngân", "Oanh", "Phong", "Quyên", "Sang", "Thảo", "Tiến",
                                     "Uyên", "Việt", "Xuân", "Yên", "Hải", "Khải", "Lâm", "Mạnh", "Nghĩa", "Phát" };

            for (int i = 1; i <= 50; i++)
            {
                students.Add(new Student
                {
                    StudentId   = i,
                    FullName    = $"{firstNames[i - 1]} Nguyễn",
                    Email       = $"student{i:D2}@lms.edu.vn",
                    DateOfBirth = new DateTime(2000 + (i % 5), (i % 12) + 1, (i % 28) + 1)
                });
            }
            modelBuilder.Entity<Student>().HasData(students);

            // ── SEED: 500 Enrollments ─────────────────────────────────────
            var enrollments = new List<Enrollment>();
            var statuses    = new[] { "Active", "Completed", "Dropped", "Pending" };
            int enrollId    = 1;
            var rng         = new Random(42);

            for (int studentId = 1; studentId <= 50; studentId++)
            {
                // Each student gets 10 enrollments (50*10 = 500)
                var coursesForStudent = Enumerable.Range(1, 20).OrderBy(_ => rng.Next()).Take(10).ToList();
                foreach (var courseId in coursesForStudent)
                {
                    enrollments.Add(new Enrollment
                    {
                        EnrollmentId = enrollId++,
                        StudentId    = studentId,
                        CourseId     = courseId,
                        EnrollDate   = new DateTime(2023, 1, 10).AddDays(rng.Next(0, 500)),
                        Status       = statuses[rng.Next(statuses.Length)]
                    });
                }
            }
            modelBuilder.Entity<Enrollment>().HasData(enrollments);
        }
    }
}
