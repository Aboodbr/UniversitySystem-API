using System.Collections.Generic;

namespace University.Domain.Entities;

public class CourseOffering
{
    public int Id { get; set; }
    public int MaxCapacity { get; set; }

    public int CourseId { get; set; } //fk
    public Course Course { get; set; } // NAV PROP

    public int SemesterId { get; set; } //FK 
    public Semester Semester { get; set; } //NAV PROP

    public int? ProfessorId { get; set; } // null bc if the value not prepare yet
    public Professor Professor { get; set; }

    public int? RoomId { get; set; } // null bc if the value not prepare yet
    public Room Room { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<CourseSchedule> Schedules { get; set; } = new List<CourseSchedule>();
}
