using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using University.Application.DTOs;
using University.Application.DTOs.Courses;
using University.Application.Interfaces;

namespace University.Api.Controllers;

public class CoursesController : BaseApiController
{
    private readonly ICourseService _courseService;

    // Dependency Injection for the Service layer only
    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// Retrieves a list of all available courses in the university catalog.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CourseResponseDto>>>> GetCourses()
    {
        var response = await _courseService.GetAllCoursesAsync();

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    /// <summary>
    /// Retrieves details of a specific course by its unique ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> GetCourse(int id)
    {
        var response = await _courseService.GetCourseByIdAsync(id);

        // If the course is not found, return a 404 Not Found status
        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}