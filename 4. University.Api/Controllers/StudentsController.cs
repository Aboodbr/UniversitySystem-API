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

    public StudentsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("{studentId}/enroll/{courseOfferingId}")]
    public async Task<ActionResult<ApiResponse<EnrollmentResponseDto>>> Enroll(int studentId, int courseOfferingId)
    {
        var result = await _registrationService.RegisterForCourseAsync(studentId, courseOfferingId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{studentId}/drop/{courseOfferingId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DropCourse(int studentId, int courseOfferingId)
    {
        var result = await _registrationService.DropCourseAsync(studentId, courseOfferingId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
