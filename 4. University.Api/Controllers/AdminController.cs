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
    private readonly IDepartmentService _departmentService;

    // Constructor for Dependency Injection
    public AdminController(ISemesterService semesterService, ICourseService courseService, IDepartmentService departmentService)
    {
        _semesterService = semesterService;
        _courseService = courseService;
        _departmentService = departmentService;
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

    [HttpPost("departments")]
    public async Task<ActionResult<ApiResponse<University.Application.DTOs.Departments.DepartmentDto>>> CreateDepartment([FromBody] University.Application.DTOs.Departments.CreateDepartmentDto dto)
    {
        var response = await _departmentService.CreateDepartmentAsync(dto);
        if (!response.Success) return BadRequest(response);
        return CreatedAtAction("GetDepartment", "Departments", new { id = response.Data?.Id }, response);
    }

    [HttpPut("departments/{id}")]
    public async Task<ActionResult<ApiResponse<University.Application.DTOs.Departments.DepartmentDto>>> UpdateDepartment(int id, [FromBody] University.Application.DTOs.Departments.UpdateDepartmentDto dto)
    {
        if (id != dto.Id) return BadRequest(new ApiResponse<University.Application.DTOs.Departments.DepartmentDto>("ID mismatch"));
        var response = await _departmentService.UpdateDepartmentAsync(dto);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("departments/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDepartment(int id)
    {
        var response = await _departmentService.DeleteDepartmentAsync(id);
        if (!response.Success) return NotFound(response);
        return Ok(response);
    }

    [HttpPut("courses/{id}")]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        var response = await _courseService.UpdateCourseAsync(id, dto);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("courses/{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(int id)
    {
        var response = await _courseService.DeleteCourseAsync(id);
        if (!response.Success) return NotFound(response);
        return Ok(response);
    }

    [HttpPut("offerings/{id}")]
    public async Task<ActionResult<ApiResponse<CourseOfferingDto>>> UpdateCourseOffering(int id, [FromBody] UpdateCourseOfferingDto dto)
    {
        var response = await _courseService.UpdateCourseOfferingAsync(id, dto);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }
}