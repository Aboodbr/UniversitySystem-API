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

    // Constructor for Dependency Injection
    public ProfessorsController(IGradingService gradingService)
    {
        _gradingService = gradingService;
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
}