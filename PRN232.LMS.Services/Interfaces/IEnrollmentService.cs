using PRN232.LMS.Repositories.Common;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<PagedList<EnrollmentModel>> GetAllAsync(ResourceQueryParameters parameters);
        Task<EnrollmentModel?> GetByIdAsync(int id);
        Task<EnrollmentModel> CreateAsync(EnrollmentModel model);
        Task<EnrollmentModel?> UpdateAsync(int id, EnrollmentModel model);
        Task<bool> DeleteAsync(int id);
    }
}

