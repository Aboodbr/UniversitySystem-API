namespace University.Application.DTOs.Students;

public class StudentResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double GPA { get; set; }
    public int CompletedHours { get; set; }
    public string AcademicStatus { get; set; }
    public string DepartmentName { get; set; }
}
