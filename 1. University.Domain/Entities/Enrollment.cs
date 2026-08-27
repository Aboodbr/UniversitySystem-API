using University.Domain.Enums;

namespace University.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; }

    public Grade? Grade { get; set; }
    public double? TotalMarks { get; set; }

    public EnrollmentStatus Status { get; set; }
}
