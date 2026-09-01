using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Courses;
using University.Application.DTOs.Offerings;
using University.Application.DTOs.Semesters;
using University.Application.Interfaces;
using University.Application.Services;

namespace University.Api.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    private readonly ISemesterService _semesterService;
    private readonly ICourseService _courseService;

    // Constructor for Dependency Injection
    public AdminController(ISemesterService semesterService, ICourseService courseService)
    {
        _semesterService = semesterService;
        _courseService = courseService;
    }

    /// <summary>
    /// Starts a new academic semester
    /// </summary>
    [HttpPost("semester/start")]
    public async Task<ActionResult<ApiResponse<bool>>> StartNewSemester(CreateSemesterDto request)
    {
        var response = await _semesterService.StartNewSemesterAsync(request);

        // Return 400 Bad Request if the operation fails (e.g., an active semester already exists)
        if (!response.Success)
            return BadRequest(response);

        // Return 200 OK if successful
        return Ok(response);
    }

    /// <summary>
    /// Adds a new course to the university catalog (e.g., CS101)
    /// </summary>
    [HttpPost("courses")]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        var response = await _courseService.CreateCourseAsync(createCourseDto);

        if (!response.Success)
            return BadRequest(response);

        // 201 Created is the golden standard when a new resource is successfully inserted into the database
        return CreatedAtAction(nameof(CreateCourse), new { id = response.Data?.Id }, response);
    }

    /// <summary>
    /// Opens a new offering for a course and assigns it to a professor and a room
    /// </summary>
    [HttpPost("offerings")]
    public async Task<ActionResult<ApiResponse<CourseOfferingDto>>> CreateCourseOffering([FromBody] CreateCourseOfferingDto createOfferingDto)
    {
        var response = await _courseService.CreateCourseOfferingAsync(createOfferingDto);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtAction(nameof(CreateCourseOffering), new { id = response.Data?.Id }, response);
    }
}