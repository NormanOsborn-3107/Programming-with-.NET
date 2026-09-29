using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<PagedList<Enrollment>> GetAllAsync(ResourceQueryParameters parameters);
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment> AddAsync(Enrollment enrollment);
        Task<Enrollment?> UpdateAsync(Enrollment enrollment);
        Task<bool> DeleteAsync(int id);
    }
}
