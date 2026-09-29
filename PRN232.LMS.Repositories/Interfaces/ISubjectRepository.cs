using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<PagedList<Subject>> GetAllAsync(ResourceQueryParameters parameters);
        Task<Subject?> GetByIdAsync(int id);
        Task<Subject> AddAsync(Subject subject);
        Task<Subject?> UpdateAsync(Subject subject);
        Task<bool> DeleteAsync(int id);
    }
}
