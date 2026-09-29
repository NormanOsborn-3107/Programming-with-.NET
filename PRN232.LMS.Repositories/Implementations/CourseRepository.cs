using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using System.Linq.Dynamic.Core;

namespace PRN232.LMS.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LMSDbContext _context;

        public CourseRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Course>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var collection = _context.Courses.AsNoTracking()
                .Include(c => c.Semester).AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                collection = collection.Where(c => c.CourseName.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Sort))
            {
                var sortQuery = string.Join(", ", parameters.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p =>
                {
                    p = p.Trim();
                    var desc = p.StartsWith("-");
                    var field = desc ? p[1..] : p;
                    
                    if (field.Equals("totalEnrollments", StringComparison.OrdinalIgnoreCase))
                    {
                        field = "Enrollments.Count";
                    }
                    else if (field.Equals("semesterName", StringComparison.OrdinalIgnoreCase))
                    {
                        field = "Semester.SemesterName";
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
                        collection = collection.OrderBy(e => e.CourseId);
                    }
                }
            }
            else { collection = collection.OrderBy(e => e.CourseId); }

            return await PagedList<Course>.CreateAsync(collection, parameters.Page, parameters.Size);
        }

        public async Task<Course?> GetByIdAsync(int id)
            => await _context.Courses.AsNoTracking().Include(c => c.Semester).FirstOrDefaultAsync(c => c.CourseId == id);

        public async Task<Course> AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> UpdateAsync(Course course)
        {
            var existing = await _context.Courses.FindAsync(course.CourseId);
            if (existing is null) return null;

            existing.CourseName  = course.CourseName;
            existing.SemesterId  = course.SemesterId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Courses.FindAsync(id);
            if (existing is null) return false;

            _context.Courses.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
