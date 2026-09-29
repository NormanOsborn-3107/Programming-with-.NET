using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<PagedList<CourseModel>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var courses = await _courseRepository.GetAllAsync(parameters);
            var mapped = courses.Select(MapToModel);
            return new PagedList<CourseModel>(mapped, courses.Pagination);
        }

        public async Task<CourseModel?> GetByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course is null ? null : MapToModel(course);
        }

        public async Task<CourseModel> CreateAsync(CourseModel model)
        {
            var entity = new Course
            {
                CourseName = model.CourseName,
                SemesterId = model.SemesterId
            };
            var created = await _courseRepository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<CourseModel?> UpdateAsync(int id, CourseModel model)
        {
            var entity = new Course
            {
                CourseId   = id,
                CourseName = model.CourseName,
                SemesterId = model.SemesterId
            };
            var updated = await _courseRepository.UpdateAsync(entity);
            return updated is null ? null : MapToModel(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _courseRepository.DeleteAsync(id);

        private static CourseModel MapToModel(Course c) => new()
        {
            CourseId         = c.CourseId,
            CourseName       = c.CourseName,
            SemesterId       = c.SemesterId,
            SemesterName     = c.Semester?.SemesterName ?? string.Empty,
            TotalEnrollments = c.Enrollments?.Count ?? 0
        };
    }
}

