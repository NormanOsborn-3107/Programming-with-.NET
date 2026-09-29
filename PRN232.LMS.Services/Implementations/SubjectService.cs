using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;

using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<PagedList<SubjectModel>> GetAllAsync(ResourceQueryParameters parameters)
        {
            var subjects = await _subjectRepository.GetAllAsync(parameters);
            var mapped = subjects.Select(MapToModel);
            return new PagedList<SubjectModel>(mapped, subjects.Pagination);
        }

        public async Task<SubjectModel?> GetByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            return subject is null ? null : MapToModel(subject);
        }

        public async Task<SubjectModel> CreateAsync(SubjectModel model)
        {
            var entity = new Subject
            {
                SubjectCode = model.SubjectCode,
                SubjectName = model.SubjectName,
                Credit      = model.Credit
            };
            var created = await _subjectRepository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<SubjectModel?> UpdateAsync(int id, SubjectModel model)
        {
            var entity = new Subject
            {
                SubjectId   = id,
                SubjectCode = model.SubjectCode,
                SubjectName = model.SubjectName,
                Credit      = model.Credit
            };
            var updated = await _subjectRepository.UpdateAsync(entity);
            return updated is null ? null : MapToModel(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _subjectRepository.DeleteAsync(id);

        private static SubjectModel MapToModel(Subject s) => new()
        {
            SubjectId   = s.SubjectId,
            SubjectCode = s.SubjectCode,
            SubjectName = s.SubjectName,
            Credit      = s.Credit
        };
    }
}

