using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.API.RequestModels;
using PRN232.LMS.API.ResponseModels;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>Manages enrollments.</summary>
    [ApiController]
    [Route("api/enrollments")]
    [Produces("application/json")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        /// <summary>Get all enrollments.</summary>
        /// <returns>Paged list of all enrollments.</returns>
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
                var result = await _enrollmentService.GetAllAsync(parameters);
                return Ok(PagedApiResponse<object>.Ok(result.Select(MapToResponse).Cast<object>(), result.Pagination));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.BadRequest(new[] { ex.Message }, "Invalid query parameters"));
            }
        }

        /// <summary>Get an enrollment by ID.</summary>
        /// <param name="id">Enrollment ID.</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _enrollmentService.GetByIdAsync(id);
            if (result is null)
                return NotFound(ApiResponse<EnrollmentResponse>.NotFound($"Enrollment {id} not found."));
            return Ok(ApiResponse<EnrollmentResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Create a new enrollment.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<EnrollmentResponse>.BadRequest(ModelState));
            var result = await _enrollmentService.CreateAsync(MapToModel(request));
            return CreatedAtAction(nameof(GetById), new { id = result.EnrollmentId },
                ApiResponse<EnrollmentResponse>.Created(MapToResponse(result)));
        }

        /// <summary>Update an existing enrollment.</summary>
        /// <param name="id">Enrollment ID.</param>
        /// <param name="request">Updated enrollment data.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<EnrollmentResponse>.BadRequest(ModelState));
            var result = await _enrollmentService.UpdateAsync(id, MapToModel(request));
            if (result is null)
                return NotFound(ApiResponse<EnrollmentResponse>.NotFound($"Enrollment {id} not found."));
            return Ok(ApiResponse<EnrollmentResponse>.Ok(MapToResponse(result)));
        }

        /// <summary>Delete an enrollment.</summary>
        /// <param name="id">Enrollment ID.</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _enrollmentService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<EnrollmentResponse>.NotFound($"Enrollment {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Deleted successfully"));
        }
        private static EnrollmentModel MapToModel(CreateEnrollmentRequest r) => new() { StudentId = r.StudentId, CourseId = r.CourseId, Status = r.Status };
        private static EnrollmentModel MapToModel(UpdateEnrollmentRequest r) => new() { StudentId = r.StudentId, CourseId = r.CourseId, Status = r.Status };
        private static EnrollmentResponse MapToResponse(EnrollmentModel m) => new() { EnrollmentId = m.EnrollmentId, StudentId = m.StudentId, StudentName = m.StudentName, CourseId = m.CourseId, CourseName = m.CourseName, EnrollDate = m.EnrollDate, Status = m.Status, Student = m.Student == null ? null : new StudentResponse { StudentId = m.Student.StudentId, FullName = m.Student.FullName, Email = m.Student.Email, DateOfBirth = m.Student.DateOfBirth, TotalEnrollments = m.Student.TotalEnrollments }, Course = m.Course == null ? null : new CourseResponse { CourseId = m.Course.CourseId, CourseName = m.Course.CourseName, SemesterId = m.Course.SemesterId, SemesterName = m.Course.SemesterName, TotalEnrollments = m.Course.TotalEnrollments } };
    }
}
