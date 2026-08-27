using University.Domain.Enums;

namespace University.Application.DTOs.Professors;

public class SubmitGradeDto
{
    public int EnrollmentId { get; set; }
    public Grade Grade { get; set; }
    public double TotalMarks { get; set; }
}
