using System;
using System.Collections.Generic;
using University.Domain.Enums;

namespace University.Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string UserId { get; set; } // Identity User ID
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    public double GPA { get; set; }
    public int CompletedHours { get; set; }
    public AcademicStatus AcademicStatus { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
