using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.API.RequestModels;
using PRN232.LMS.API.ResponseModels;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>Manages subjects.</summary>
    [ApiController]
    [Route("api/subjects")]
    [Produces("application/json")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>Get all subjects.</summary>
        /// <returns>Paged list of all subjects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] ResourceQueryParameters parameters)
        {
            if (parameters.Page < 1 || parameters.Size < 1)
                return BadRequest(ApiResponse<object>.BadRequest(
                    new[] { "page must be >= 1 and size must be >= 1" }, "Invalid query parameters"));

            try
            {
                var result = await _subjectService.GetAllAsync(parameters);
                return Ok(PagedApiResponse<object>.Ok(result.Select(MapToResponse).Cast<object>(), result.Pagination));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.BadRequest(new[] { ex.Message }, "Invalid query parameters"));
            }
        }

        /// <summary>Get a subject by ID.</summary>
        /// <param name="id">Subject ID.</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _subjectService.GetByIdAsync(id);
            if (result is null)
                return NotFound(ApiResponse<SubjectResponse>.NotFound($"Subject {id} not found."));
            return Ok(ApiResponse<SubjectResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Create a new subject.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSubjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SubjectResponse>.BadRequest(ModelState));
            var result = await _subjectService.CreateAsync(MapToModel(request));
            return CreatedAtAction(nameof(GetById), new { id = result.SubjectId },
                ApiResponse<SubjectResponse>.Created(MapToResponse(result)));
        }

        /// <summary>Update an existing subject.</summary>
        /// <param name="id">Subject ID.</param>
        /// <param name="request">Updated subject data.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SubjectResponse>.BadRequest(ModelState));
            var result = await _subjectService.UpdateAsync(id, MapToModel(request));
            if (result is null)
                return NotFound(ApiResponse<SubjectResponse>.NotFound($"Subject {id} not found."));
            return Ok(ApiResponse<SubjectResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Delete a subject.</summary>
        /// <param name="id">Subject ID.</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _subjectService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<SubjectResponse>.NotFound($"Subject {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Deleted successfully"));
        }

        private static SubjectModel MapToModel(CreateSubjectRequest r) => new() { SubjectCode = r.SubjectCode, SubjectName = r.SubjectName, Credit = r.Credit };
        private static SubjectModel MapToModel(UpdateSubjectRequest r) => new() { SubjectCode = r.SubjectCode, SubjectName = r.SubjectName, Credit = r.Credit };
        private static SubjectResponse MapToResponse(SubjectModel m) => new() { SubjectId = m.SubjectId, SubjectCode = m.SubjectCode, SubjectName = m.SubjectName, Credit = m.Credit };
    }
}
