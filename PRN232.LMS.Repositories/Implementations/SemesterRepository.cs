using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using System.Linq.Dynamic.Core;

namespace PRN232.LMS.Repositories.Implementations
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly LMSDbContext _context;

        public SemesterRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Semester>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var collection = _context.Semesters.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                collection = collection.Where(s => s.SemesterName.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Sort))
            {
                var sortParts = parameters.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var sortQuery = string.Join(", ", sortParts.Select(p =>
                {
                    p = p.Trim();
                    var desc = p.StartsWith("-");
                    var field = desc ? p[1..] : p;
                    
                    if (field.Equals("totalCourses", StringComparison.OrdinalIgnoreCase))
                    {
                        field = "Courses.Count";
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
                        collection = collection.OrderBy(e => e.SemesterId);
                    }
                }
            }
            else { collection = collection.OrderBy(e => e.SemesterId); }

            return await PagedList<Semester>.CreateAsync(collection, parameters.Page, parameters.Size);
        }

        public async Task<Semester?> GetByIdAsync(int id)
            => await _context.Semesters.AsNoTracking().FirstOrDefaultAsync(s => s.SemesterId == id);

        public async Task<Semester> AddAsync(Semester semester)
        {
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();
            return semester;
        }

        public async Task<Semester?> UpdateAsync(Semester semester)
        {
            var existing = await _context.Semesters.FindAsync(semester.SemesterId);
            if (existing is null) return null;

            existing.SemesterName = semester.SemesterName;
            existing.StartDate    = semester.StartDate;
            existing.EndDate      = semester.EndDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Semesters.FindAsync(id);
            if (existing is null) return false;

            _context.Semesters.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

