using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<PagedList<Student>> GetAllAsync(ResourceQueryParameters parameters);
        Task<Student?> GetByIdAsync(int id, string? expand = null);
        Task<Student> AddAsync(Student student);
        Task<Student?> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
