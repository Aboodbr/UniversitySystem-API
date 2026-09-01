using System.ComponentModel.DataAnnotations;

namespace University.Application.DTOs.Courses;

public class UpdateCourseDto
{
    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    [Range(1, 10)]
    public int Credits { get; set; }
}
