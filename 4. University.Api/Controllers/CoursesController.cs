using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using University.Application.DTOs;
using University.Application.DTOs.Courses;
using University.Application.Interfaces;
using AutoMapper;

namespace University.Api.Controllers;

public class CoursesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CourseResponseDto>>>> GetCourses()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var data = _mapper.Map<IReadOnlyList<CourseResponseDto>>(courses);
        return Ok(new ApiResponse<IReadOnlyList<CourseResponseDto>>(data));
    }
}
