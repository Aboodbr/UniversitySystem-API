using System.Collections.Generic;
using University.Application.DTOs.Courses;

namespace University.Application.DTOs.Departments;

public class DepartmentDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<CourseResponseDto> Courses { get; set; } = new List<CourseResponseDto>();
}
