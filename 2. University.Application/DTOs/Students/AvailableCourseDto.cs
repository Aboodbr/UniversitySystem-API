namespace University.Application.DTOs.Students;

public class AvailableCourseDto
{
    public int CourseOfferingId { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }
    public int Credits { get; set; }
    public string ProfessorName { get; set; }
    public int AvailableSeats { get; set; }
}
