using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.API.RequestModels;
using PRN232.LMS.API.ResponseModels;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>Manages semesters.</summary>
    [ApiController]
    [Route("api/semesters")]
    [Produces("application/json")]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        /// <summary>Get all semesters.</summary>
        /// <returns>Paged list of all semesters.</returns>
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
                var result = await _semesterService.GetAllAsync(parameters);
                return Ok(PagedApiResponse<object>.Ok(result.Select(MapToResponse).Cast<object>(), result.Pagination));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.BadRequest(new[] { ex.Message }, "Invalid query parameters"));
            }
        }

        /// <summary>Get a semester by ID.</summary>
        /// <param name="id">Semester ID.</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _semesterService.GetByIdAsync(id);
            if (result is null)
                return NotFound(ApiResponse<SemesterResponse>.NotFound($"Semester {id} not found."));
            return Ok(ApiResponse<SemesterResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Create a new semester.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSemesterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SemesterResponse>.BadRequest(ModelState));
            var result = await _semesterService.CreateAsync(MapToModel(request));
            return CreatedAtAction(nameof(GetById), new { id = result.SemesterId },
                ApiResponse<SemesterResponse>.Created(MapToResponse(result)));
        }

        /// <summary>Update an existing semester.</summary>
        /// <param name="id">Semester ID.</param>
        /// <param name="request">Updated semester data.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSemesterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<SemesterResponse>.BadRequest(ModelState));
            var result = await _semesterService.UpdateAsync(id, MapToModel(request));
            if (result is null)
                return NotFound(ApiResponse<SemesterResponse>.NotFound($"Semester {id} not found."));
            return Ok(ApiResponse<SemesterResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Delete a semester.</summary>
        /// <param name="id">Semester ID.</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _semesterService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<SemesterResponse>.NotFound($"Semester {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Deleted successfully"));
        }
        private static SemesterModel MapToModel(CreateSemesterRequest r) => new() { SemesterName = r.SemesterName, StartDate = r.StartDate, EndDate = r.EndDate };
        private static SemesterModel MapToModel(UpdateSemesterRequest r) => new() { SemesterName = r.SemesterName, StartDate = r.StartDate, EndDate = r.EndDate };
        private static SemesterResponse MapToResponse(SemesterModel m) => new() { SemesterId = m.SemesterId, SemesterName = m.SemesterName, StartDate = m.StartDate, EndDate = m.EndDate, TotalCourses = m.TotalCourses };
    }
}
