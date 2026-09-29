using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<PagedList<EnrollmentModel>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var enrollments = await _enrollmentRepository.GetAllAsync(parameters);
            var mapped = enrollments.Select(MapToModel);
            return new PagedList<EnrollmentModel>(mapped, enrollments.Pagination);
        }

        public async Task<EnrollmentModel?> GetByIdAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            return enrollment is null ? null : MapToModel(enrollment);
        }

        public async Task<EnrollmentModel> CreateAsync(EnrollmentModel model)
        {
            var entity = new Enrollment
            {
                StudentId  = model.StudentId,
                CourseId   = model.CourseId,
                EnrollDate = model.EnrollDate,
                Status     = model.Status
            };
            var created = await _enrollmentRepository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<EnrollmentModel?> UpdateAsync(int id, EnrollmentModel model)
        {
            var entity = new Enrollment
            {
                EnrollmentId = id,
                StudentId    = model.StudentId,
                CourseId     = model.CourseId,
                EnrollDate   = model.EnrollDate,
                Status       = model.Status
            };
            var updated = await _enrollmentRepository.UpdateAsync(entity);
            return updated is null ? null : MapToModel(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _enrollmentRepository.DeleteAsync(id);

        private static EnrollmentModel MapToModel(Enrollment e) => new()
        {
            EnrollmentId = e.EnrollmentId,
            StudentId    = e.StudentId,
            StudentName  = e.Student?.FullName ?? string.Empty,
            CourseId     = e.CourseId,
            CourseName   = e.Course?.CourseName ?? string.Empty,
            EnrollDate   = e.EnrollDate,
            Status       = e.Status,
            Student      = e.Student == null ? null : new StudentModel 
            {
                StudentId = e.Student.StudentId,
                FullName = e.Student.FullName,
                Email = e.Student.Email,
                DateOfBirth = e.Student.DateOfBirth,
                Age = DateTime.Today.Year - e.Student.DateOfBirth.Year,
                TotalEnrollments = e.Student.Enrollments?.Count ?? 0
            },
            Course       = e.Course == null ? null : new CourseModel
            {
                CourseId = e.Course.CourseId,
                CourseName = e.Course.CourseName,
                SemesterId = e.Course.SemesterId,
                SemesterName = e.Course.Semester?.SemesterName ?? string.Empty,
                TotalEnrollments = e.Course.Enrollments?.Count ?? 0
            }
        };
    }
}

