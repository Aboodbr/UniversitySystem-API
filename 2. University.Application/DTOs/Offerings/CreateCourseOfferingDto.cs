namespace University.Application.DTOs.Offerings;

public class CreateCourseOfferingDto
{
    public int MaxCapacity { get; set; }
    public int CourseId { get; set; }
    public int SemesterId { get; set; }
    public int? ProfessorId { get; set; }
    public int? RoomId { get; set; }
}
