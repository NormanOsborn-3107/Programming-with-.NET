using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using System.Linq.Dynamic.Core;

namespace PRN232.LMS.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly LMSDbContext _context;

        public SubjectRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Subject>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var collection = _context.Subjects.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                collection = collection.Where(s => s.SubjectName.ToLower().Contains(search) ||
                                                   s.SubjectCode.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Sort))
            {
                var sortQuery = string.Join(", ", parameters.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p =>
                {
                    p = p.Trim();
                    var desc = p.StartsWith("-");
                    var field = desc ? p[1..] : p;
                    if (field.Length > 0)
                        field = char.ToUpper(field[0]) + field.Substring(1);
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
                        collection = collection.OrderBy(e => e.SubjectId);
                    }
                }
            }
            else { collection = collection.OrderBy(e => e.SubjectId); }

            return await PagedList<Subject>.CreateAsync(collection, parameters.Page, parameters.Size);
        }

        public async Task<Subject?> GetByIdAsync(int id)
            => await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.SubjectId == id);

        public async Task<Subject> AddAsync(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject;
        }

        public async Task<Subject?> UpdateAsync(Subject subject)
        {
            var existing = await _context.Subjects.FindAsync(subject.SubjectId);
            if (existing is null) return null;

            existing.SubjectCode = subject.SubjectCode;
            existing.SubjectName = subject.SubjectName;
            existing.Credit      = subject.Credit;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Subjects.FindAsync(id);
            if (existing is null) return false;

            _context.Subjects.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
