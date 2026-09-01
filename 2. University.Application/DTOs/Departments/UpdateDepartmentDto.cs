using System.ComponentModel.DataAnnotations;

namespace University.Application.DTOs.Departments;

public class UpdateDepartmentDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name Of Department is Requierd.")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Code Of Department is Requierd.")]
    [MaxLength(10)]
    public string Code { get; set; }
}