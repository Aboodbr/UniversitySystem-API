namespace University.Application.DTOs.Offerings;

public class CourseOfferingDto
{
    public int Id { get; set; }
    public int MaxCapacity { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }
    public string SemesterName { get; set; }
    public string ProfessorName { get; set; }
    public string RoomName { get; set; }
}
