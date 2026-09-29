using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterService(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        public async Task<PagedList<SemesterModel>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var semesters = await _semesterRepository.GetAllAsync(parameters);
            var mapped = semesters.Select(MapToModel);
            return new PagedList<SemesterModel>(mapped, semesters.Pagination);
        }

        public async Task<SemesterModel?> GetByIdAsync(int id)
        {
            var semester = await _semesterRepository.GetByIdAsync(id);
            return semester is null ? null : MapToModel(semester);
        }

        public async Task<SemesterModel> CreateAsync(SemesterModel model)
        {
            var entity = new Semester
            {
                SemesterName = model.SemesterName,
                StartDate    = model.StartDate,
                EndDate      = model.EndDate
            };
            var created = await _semesterRepository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<SemesterModel?> UpdateAsync(int id, SemesterModel model)
        {
            var entity = new Semester
            {
                SemesterId   = id,
                SemesterName = model.SemesterName,
                StartDate    = model.StartDate,
                EndDate      = model.EndDate
            };
            var updated = await _semesterRepository.UpdateAsync(entity);
            return updated is null ? null : MapToModel(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _semesterRepository.DeleteAsync(id);

        // ── Private mapping: Entity → Response ───────────────────────────
        private static SemesterModel MapToModel(Semester s) => new()
        {
            SemesterId   = s.SemesterId,
            SemesterName = s.SemesterName,
            StartDate    = s.StartDate,
            EndDate      = s.EndDate,
            TotalCourses = s.Courses?.Count ?? 0
        };
    }
}

