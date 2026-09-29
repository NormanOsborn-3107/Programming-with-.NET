using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        Task<PagedList<Semester>> GetAllAsync(ResourceQueryParameters parameters);
        Task<Semester?> GetByIdAsync(int id);
        Task<Semester> AddAsync(Semester semester);
        Task<Semester?> UpdateAsync(Semester semester);
        Task<bool> DeleteAsync(int id);
    }
}
