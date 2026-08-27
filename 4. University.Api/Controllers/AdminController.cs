using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using University.Application.DTOs;

namespace University.Api.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    [HttpPost("semester/start")]
    public async Task<ActionResult<ApiResponse<bool>>> StartNewSemester()
    {
        // Placeholder
        return Ok(new ApiResponse<bool>(true, "Semester started successfully"));
    }
}
