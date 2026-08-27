namespace University.Application.DTOs.Professors;

public class ProfessorCourseDto
{
    public int CourseOfferingId { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }
    public int EnrolledStudentsCount { get; set; }
}
