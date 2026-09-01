using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Departments;
using University.Application.Interfaces;

namespace University.Api.Controllers;

[AllowAnonymous]
public class DepartmentsController : BaseApiController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DepartmentDto>>>> GetDepartments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _departmentService.GetAllDepartmentsAsync(pageNumber, pageSize);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentDetailsDto>>> GetDepartment(int id)
    {
        var response = await _departmentService.GetDepartmentByIdAsync(id);
        if (!response.Success) return NotFound(response);
        return Ok(response);
    }
}
