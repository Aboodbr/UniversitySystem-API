using System.Collections.Generic;

namespace University.Application.DTOs.Students;

public class TranscriptDto
{
    public string StudentName { get; set; }
    public double OverallGPA { get; set; }
    public int TotalCompletedHours { get; set; }
    public List<EnrollmentResponseDto> CompletedCourses { get; set; } = new List<EnrollmentResponseDto>();
}
