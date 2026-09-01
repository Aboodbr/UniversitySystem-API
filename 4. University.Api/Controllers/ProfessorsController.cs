using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Professors;
using University.Application.Interfaces;
using University.Application.Services;

namespace University.Api.Controllers;

[Authorize(Roles = "Professor,Admin")]
public class ProfessorsController : BaseApiController
{
    private readonly IGradingService _gradingService;
    private readonly IProfessorService _professorService;

    // Constructor for Dependency Injection
    public ProfessorsController(IGradingService gradingService, IProfessorService professorService)
    {
        _gradingService = gradingService;
        _professorService = professorService;
    }

    /// <summary>
    /// Submits a final grade for a student in a specific course offering.
    /// </summary>
    /// <param name="professorId">The unique identifier of the professor.</param>
    /// <param name="submitGradeDto">The grading details including enrollment ID and total marks.</param>
    /// <returns>A standard API response indicating success or failure.</returns>
    [HttpPost("{professorId}/grades")]
    public async Task<ActionResult<ApiResponse<bool>>> SubmitGrade([FromRoute] int professorId, [FromBody] SubmitGradeDto submitGradeDto)
    {
        // Delegate the business logic to the Grading Service
        var result = await _gradingService.SubmitGradeAsync(submitGradeDto, professorId);

        // Return 400 Bad Request if the grading fails (e.g., unauthorized professor, withdrawn student)
        if (!result.Success)
            return BadRequest(result);

        // Return 200 OK if the grade is successfully recorded and GPA is updated
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Student")]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.IReadOnlyList<ProfessorResponseDto>>>> GetProfessors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _professorService.GetAllProfessorsAsync(pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("{id}/schedule")]
    [Authorize(Roles = "Professor,Admin")]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.IReadOnlyList<University.Application.DTOs.Offerings.CourseOfferingDto>>>> GetProfessorSchedule(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _professorService.GetProfessorScheduleAsync(id, pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("offerings/{offeringId}/students")]
    [Authorize(Roles = "Professor")]
    public async Task<ActionResult<ApiResponse<System.Collections.Generic.IReadOnlyList<University.Application.DTOs.Students.StudentResponseDto>>>> GetOfferingStudents(int offeringId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _professorService.GetOfferingStudentsAsync(offeringId, pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }
}