
using PRN232.LMS.Services.BusinessModels;

using PRN232.LMS.Repositories.Common;
using System.Dynamic;

namespace PRN232.LMS.Services.Interfaces
{
    public interface IStudentService
    {
        Task<PagedList<ExpandoObject>> GetAllAsync(ResourceQueryParameters parameters);
        Task<ExpandoObject?> GetByIdAsync(int id, string? fields = null, string? expand = null);
        Task<StudentModel> CreateAsync(StudentModel model);
        Task<StudentModel?> UpdateAsync(int id, StudentModel model);
        Task<bool> DeleteAsync(int id);
    }
}

