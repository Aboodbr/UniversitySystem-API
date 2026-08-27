namespace University.Application.DTOs.Courses;

public class CourseResponseDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public string DepartmentName { get; set; }
}
