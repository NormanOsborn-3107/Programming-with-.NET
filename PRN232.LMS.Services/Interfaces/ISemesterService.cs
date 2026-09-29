using PRN232.LMS.Repositories.Common;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<PagedList<SemesterModel>> GetAllAsync(ResourceQueryParameters parameters);
        Task<SemesterModel?> GetByIdAsync(int id);
        Task<SemesterModel> CreateAsync(SemesterModel model);
        Task<SemesterModel?> UpdateAsync(int id, SemesterModel model);
        Task<bool> DeleteAsync(int id);
    }
}

