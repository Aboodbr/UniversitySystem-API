using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Students;
using University.Application.Services;

namespace University.Api.Controllers;

[Authorize(Roles = "Student,Admin")]
public class StudentsController : BaseApiController
{
    private readonly IRegistrationService _registrationService;

    // Constructor for Dependency Injection
    public StudentsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
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
}