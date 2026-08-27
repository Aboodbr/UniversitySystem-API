namespace University.Application.DTOs.Students;

public class EnrollmentResponseDto
{
    public int Id { get; set; }
    public int CourseOfferingId { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }
    public string Status { get; set; }
    public string Grade { get; set; }
}
