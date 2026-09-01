using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Students;
using University.Application.Interfaces;
using University.Application.Services;

namespace University.Api.Controllers;

[Authorize(Roles = "Student,Admin")]
public class StudentsController : BaseApiController
{
    private readonly IRegistrationService _registrationService;
    private readonly IStudentService _studentService;

    // Constructor for Dependency Injection
    public StudentsController(IRegistrationService registrationService, IStudentService studentService)
    {
        _registrationService = registrationService;
        _studentService = studentService;
    }

    /// <summary>
    /// Registers a student for a specific course offering.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="courseOfferingId">The unique identifier of the course offering (section).</param>
    /// <returns>A standard API response containing the enrollment details.</returns>
    [HttpPost("{studentId}/enroll/{courseOfferingId}")]
    public async Task<ActionResult<ApiResponse<EnrollmentResponseDto>>> Enroll([FromRoute] int studentId, [FromRoute] int courseOfferingId)
    {
        // Delegate the complex registration logic (capacity, prerequisites, credits) to the Application layer
        var result = await _registrationService.RegisterForCourseAsync(studentId, courseOfferingId);

        // Return 400 Bad Request if any business rule fails
        if (!result.Success)
            return BadRequest(result);

        // Return 200 OK if the student is successfully enrolled
        return Ok(result);
    }

    /// <summary>
    /// Drops (withdraws) a student from an active course offering.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="courseOfferingId">The unique identifier of the course offering (section).</param>
    /// <returns>A standard API response indicating the success of the operation.</returns>
    [HttpDelete("{studentId}/drop/{courseOfferingId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DropCourse([FromRoute] int studentId, [FromRoute] int courseOfferingId)
    {
        // Delegate the withdrawal logic to the Application layer (changes status to Withdrawn)
        var result = await _registrationService.DropCourseAsync(studentId, courseOfferingId);

        // Return 400 Bad Request if the drop fails (e.g., enrollment not found)
        if (!result.Success)
            return BadRequest(result);

        // Return 200 OK if successfully dropped
        return Ok(result);
    }

    [HttpGet("{id}/schedule")]
    [Authorize(Roles = "Student,Admin")]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.IReadOnlyList<EnrollmentResponseDto>>>> GetStudentSchedule(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _studentService.GetStudentScheduleAsync(id, pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("{id}/transcript")]
    [Authorize(Roles = "Student,Admin")]
    public async Task<ActionResult<ApiResponse<TranscriptDto>>> GetStudentTranscript(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _studentService.GetStudentTranscriptAsync(id, pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("{id}/progress")]
    [Authorize(Roles = "Student,Admin")]
    public async Task<ActionResult<ApiResponse<StudentProfileDto>>> GetStudentProgress(int id)
    {
        var response = await _studentService.GetStudentProgressAsync(id);
        if (!response.Success) return NotFound(response);
        return Ok(response);
    }
}