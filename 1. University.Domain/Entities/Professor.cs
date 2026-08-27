using System.Collections.Generic;

namespace University.Domain.Entities;

public class Professor
{
    public int Id { get; set; }
    public string UserId { get; set; } // Identity User ID
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}
