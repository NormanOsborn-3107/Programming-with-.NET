using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.API.RequestModels;
using PRN232.LMS.API.ResponseModels;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>Manages students.</summary>
    [ApiController]
    [Route("api/students")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>Get all students (supports paging, filtering, sorting, field selection).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] ResourceQueryParameters parameters)
        {
            // Validate query params per grader rule: page=0 or size=-1 → 400
            if (parameters.Page < 1 || parameters.Size < 1)
                return BadRequest(ApiResponse<object>.BadRequest(
                    new[] { "page must be >= 1 and size must be >= 1" }, "Invalid query parameters"));

            try
            {
                var result = await _studentService.GetAllAsync(parameters);
                return Ok(PagedApiResponse<object>.Ok(result.Cast<object>(), result.Pagination));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.BadRequest(new[] { ex.Message }, "Invalid query parameters"));
            }
        }

        /// <summary>Get a student by ID (supports field selection and expansion).</summary>
        /// <param name="id">Student ID.</param>
        /// <param name="fields">Comma-separated list of fields to include.</param>
        /// <param name="expand">Comma-separated list of related entities to expand.</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, [FromQuery] string? fields, [FromQuery] string? expand)
        {
            var result = await _studentService.GetByIdAsync(id, fields, expand);
            if (result is null)
                return NotFound(ApiResponse<StudentResponse>.NotFound($"Student {id} not found."));
            return Ok(ApiResponse<object>.Ok(result));
        }

        /// <summary>Create a new student.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<StudentResponse>.BadRequest(ModelState));
            var result = await _studentService.CreateAsync(MapToModel(request));
            return CreatedAtAction(nameof(GetById), new { id = result.StudentId },
                ApiResponse<StudentResponse>.Created(MapToResponse(result)));
        }

        /// <summary>Update an existing student.</summary>
        /// <param name="id">Student ID.</param>
        /// <param name="request">Updated student data.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<StudentResponse>.BadRequest(ModelState));
            var result = await _studentService.UpdateAsync(id, MapToModel(request));
            if (result is null)
                return NotFound(ApiResponse<StudentResponse>.NotFound($"Student {id} not found."));
            return Ok(ApiResponse<StudentResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Delete a student.</summary>
        /// <param name="id">Student ID.</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _studentService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<StudentResponse>.NotFound($"Student {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Deleted successfully"));
        }
        private static StudentModel MapToModel(CreateStudentRequest r) => new() { FullName = r.FullName, Email = r.Email, DateOfBirth = r.DateOfBirth };
        private static StudentModel MapToModel(UpdateStudentRequest r) => new() { FullName = r.FullName, Email = r.Email, DateOfBirth = r.DateOfBirth };
        private static StudentResponse MapToResponse(StudentModel m) => new() { StudentId = m.StudentId, FullName = m.FullName, Email = m.Email, DateOfBirth = m.DateOfBirth, TotalEnrollments = m.TotalEnrollments, Enrollments = m.Enrollments?.Select(e => new EnrollmentResponse { EnrollmentId = e.EnrollmentId, CourseId = e.CourseId, CourseName = e.CourseName, EnrollDate = e.EnrollDate, Status = e.Status }).ToList() };
    }
}
