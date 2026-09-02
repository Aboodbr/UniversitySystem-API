namespace University.Application.DTOs.Auth;

public class RegisterRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; } // "Student", "Professor"
    public DateTime DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
}
