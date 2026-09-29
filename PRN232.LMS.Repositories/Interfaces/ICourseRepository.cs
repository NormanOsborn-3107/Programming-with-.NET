using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<PagedList<Course>> GetAllAsync(ResourceQueryParameters parameters);
        Task<Course?> GetByIdAsync(int id);
        Task<Course> AddAsync(Course course);
        Task<Course?> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}
