using System.Collections.Generic;

namespace University.Domain.Entities;

public class CourseOffering
{
    public int Id { get; set; }
    public int MaxCapacity { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int SemesterId { get; set; }
    public Semester Semester { get; set; }

    public int? ProfessorId { get; set; }
    public Professor Professor { get; set; }

    public int? RoomId { get; set; }
    public Room Room { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<CourseSchedule> Schedules { get; set; } = new List<CourseSchedule>();
}
