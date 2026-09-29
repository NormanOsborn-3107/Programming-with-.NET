using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.API.RequestModels;
using PRN232.LMS.API.ResponseModels;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>Manages courses.</summary>
    [ApiController]
    [Route("api/courses")]
    [Produces("application/json")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>Get all courses.</summary>
        /// <returns>Paged list of all courses.</returns>
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
                var result = await _courseService.GetAllAsync(parameters);
                return Ok(PagedApiResponse<object>.Ok(result.Select(MapToResponse).Cast<object>(), result.Pagination));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.BadRequest(new[] { ex.Message }, "Invalid query parameters"));
            }
        }

        /// <summary>Get a course by ID.</summary>
        /// <param name="id">Course ID.</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (result is null)
                return NotFound(ApiResponse<CourseResponse>.NotFound($"Course {id} not found."));
            return Ok(ApiResponse<CourseResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Create a new course.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<CourseResponse>.BadRequest(ModelState));
            var result = await _courseService.CreateAsync(MapToModel(request));
            return CreatedAtAction(nameof(GetById), new { id = result.CourseId },
                ApiResponse<CourseResponse>.Created(MapToResponse(result)));
        }

        /// <summary>Update an existing course.</summary>
        /// <param name="id">Course ID.</param>
        /// <param name="request">Updated course data.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<CourseResponse>.BadRequest(ModelState));
            var result = await _courseService.UpdateAsync(id, MapToModel(request));
            if (result is null)
                return NotFound(ApiResponse<CourseResponse>.NotFound($"Course {id} not found."));
            return Ok(ApiResponse<CourseResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Delete a course.</summary>
        /// <param name="id">Course ID.</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _courseService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<CourseResponse>.NotFound($"Course {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Deleted successfully"));
        }

        private static CourseModel MapToModel(CreateCourseRequest r) => new() { CourseName = r.CourseName, SemesterId = r.SemesterId };
        private static CourseModel MapToModel(UpdateCourseRequest r) => new() { CourseName = r.CourseName, SemesterId = r.SemesterId };
        private static CourseResponse MapToResponse(CourseModel m) => new() { CourseId = m.CourseId, CourseName = m.CourseName, SemesterId = m.SemesterId, SemesterName = m.SemesterName, TotalEnrollments = m.TotalEnrollments };
    }
}
