using PRN232.LMS.Repositories.Common;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<PagedList<SubjectModel>> GetAllAsync(ResourceQueryParameters parameters);
        Task<SubjectModel?> GetByIdAsync(int id);
        Task<SubjectModel> CreateAsync(SubjectModel model);
        Task<SubjectModel?> UpdateAsync(int id, SubjectModel model);
        Task<bool> DeleteAsync(int id);
    }
}

