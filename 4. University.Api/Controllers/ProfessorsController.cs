using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Professors;
using University.Application.Services;

namespace University.Api.Controllers;

[Authorize(Roles = "Professor,Admin")]
public class ProfessorsController : BaseApiController
{
    private readonly IGradingService _gradingService;

    public ProfessorsController(IGradingService gradingService)
    {
        _gradingService = gradingService;
    }

    [HttpPost("{professorId}/grades")]
    public async Task<ActionResult<ApiResponse<bool>>> SubmitGrade(int professorId, SubmitGradeDto submitGradeDto)
    {
        var result = await _gradingService.SubmitGradeAsync(submitGradeDto, professorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
