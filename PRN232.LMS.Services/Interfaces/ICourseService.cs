using PRN232.LMS.Repositories.Common;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Interfaces
{
    public interface ICourseService
    {
        Task<PagedList<CourseModel>> GetAllAsync(ResourceQueryParameters parameters);
        Task<CourseModel?> GetByIdAsync(int id);
        Task<CourseModel> CreateAsync(CourseModel model);
        Task<CourseModel?> UpdateAsync(int id, CourseModel model);
        Task<bool> DeleteAsync(int id);
    }
}

