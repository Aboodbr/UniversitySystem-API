namespace University.Application.DTOs.Courses;

public class CreateCourseDto
{
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public int? PrerequisiteCourseId { get; set; }
}
