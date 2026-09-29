using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.Interfaces;

using PRN232.LMS.Services.BusinessModels;
using System.Dynamic;

namespace PRN232.LMS.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<PagedList<ExpandoObject>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var pagedStudents = await _studentRepository.GetAllAsync(parameters);

            // Map to response DTO first (so expanded navigation props are included)
            var mapped = pagedStudents.Select(s => new StudentModel
            {
                StudentId        = s.StudentId,
                FullName         = s.FullName,
                Email            = s.Email,
                DateOfBirth      = s.DateOfBirth,
                
                TotalEnrollments = s.Enrollments?.Count ?? 0,
                Enrollments      = s.Enrollments?.Select(e => new EnrollmentModel
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollDate = e.EnrollDate,
                    Status = e.Status
                }).ToList()
            });

            // Shape data based on fields
            var shapedData = mapped.ShapeData(parameters.Fields);

            return new PagedList<ExpandoObject>(shapedData, pagedStudents.Pagination);
        }

        public async Task<ExpandoObject?> GetByIdAsync(int id, string? fields = null, string? expand = null)
        {
            var student = await _studentRepository.GetByIdAsync(id, expand);
            if (student == null) return null;

            var mapped = new StudentModel
            {
                StudentId        = student.StudentId,
                FullName         = student.FullName,
                Email            = student.Email,
                DateOfBirth      = student.DateOfBirth,
                
                TotalEnrollments = student.Enrollments?.Count ?? 0,
                Enrollments      = student.Enrollments?.Select(e => new EnrollmentModel
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollDate = e.EnrollDate,
                    Status = e.Status
                }).ToList()
            };

            return mapped.ShapeData(fields);
        }

        public async Task<StudentModel> CreateAsync(StudentModel model)
        {
            var entity = new Student
            {
                FullName    = model.FullName,
                Email       = model.Email,
                DateOfBirth = model.DateOfBirth
            };
            var created = await _studentRepository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<StudentModel?> UpdateAsync(int id, StudentModel model)
        {
            var entity = new Student
            {
                StudentId   = id,
                FullName    = model.FullName,
                Email       = model.Email,
                DateOfBirth = model.DateOfBirth
            };
            var updated = await _studentRepository.UpdateAsync(entity);
            return updated is null ? null : MapToModel(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _studentRepository.DeleteAsync(id);

        private static StudentModel MapToModel(Student s) => new()
        {
            StudentId        = s.StudentId,
            FullName         = s.FullName,
            Email            = s.Email,
            DateOfBirth      = s.DateOfBirth,
            
            TotalEnrollments = s.Enrollments?.Count ?? 0
        };
    }
}

