using System.Collections.Generic;

namespace University.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }

    public bool IsDeleted { get; set; } = false;

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Professor> Professors { get; set; } = new List<Professor>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
