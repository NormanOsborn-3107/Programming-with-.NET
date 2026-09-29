using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using System.Linq.Dynamic.Core;

namespace PRN232.LMS.Repositories.Implementations
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly LMSDbContext _context;

        public EnrollmentRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Enrollment>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var collection = _context.Enrollments.AsNoTracking().AsQueryable();

            // Expand: include student/course only when requested
            if (!string.IsNullOrWhiteSpace(parameters.Expand))
            {
                var parts = parameters.Expand.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var name = part.Trim().ToLower();
                    if (name == "student") collection = collection.Include(e => e.Student);
                    else if (name == "course") collection = collection.Include(e => e.Course);
                }
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                collection = collection.Where(e => e.Status.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Sort))
            {
                var sortQuery = string.Join(", ", parameters.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p =>
                {
                    p = p.Trim();
                    var desc = p.StartsWith("-");
                    var field = desc ? p[1..] : p;
                    
                    if (field.Equals("studentName", StringComparison.OrdinalIgnoreCase))
                    {
                        field = "Student.FullName";
                    }
                    else if (field.Equals("courseName", StringComparison.OrdinalIgnoreCase))
                    {
                        field = "Course.CourseName";
                    }
                    else if (field.Length > 0)
                    {
                        field = char.ToUpper(field[0]) + field.Substring(1);
                    }
                    
                    return $"{field} {(desc ? "descending" : "ascending")}";
                }));
                if (!string.IsNullOrWhiteSpace(sortQuery))
                {
                    try
                    {
                        collection = collection.OrderBy(sortQuery);
                    }
                    catch (System.Linq.Dynamic.Core.Exceptions.ParseException)
                    {
                        throw new ArgumentException("Invalid sort field");
                    }
                    catch
                    {
                        collection = collection.OrderBy(e => e.EnrollmentId);
                    }
                }
            }
            else { collection = collection.OrderBy(e => e.EnrollmentId); }

            return await PagedList<Enrollment>.CreateAsync(collection, parameters.Page, parameters.Size);
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
            => await _context.Enrollments.AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

        public async Task<Enrollment> AddAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment?> UpdateAsync(Enrollment enrollment)
        {
            var existing = await _context.Enrollments.FindAsync(enrollment.EnrollmentId);
            if (existing is null) return null;

            existing.StudentId  = enrollment.StudentId;
            existing.CourseId   = enrollment.CourseId;
            existing.EnrollDate = enrollment.EnrollDate;
            existing.Status     = enrollment.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Enrollments.FindAsync(id);
            if (existing is null) return false;

            _context.Enrollments.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
