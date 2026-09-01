using University.Domain.Enums;

namespace University.Domain.Entities;

// M:M RELATION 
public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; } //FK
    public Student Student { get; set; } //NAV PROP 

    public int CourseOfferingId { get; set; } //FK
    public CourseOffering CourseOffering { get; set; } //NAV PROP

    public Grade? Grade { get; set; }
    public double? TotalMarks { get; set; }

    public EnrollmentStatus Status { get; set; }
}
