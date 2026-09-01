using System.Collections.Generic;

namespace University.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }

    public bool IsDeleted { get; set; } = false;

    public int DepartmentId { get; set; } //FK 
    public Department Department { get; set; } //NAV PROP

    public int? PrerequisiteCourseId { get; set; } //IF DEPEND ON OTHER COURSE
    public Course PrerequisiteCourse { get; set; } 

    public ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}
