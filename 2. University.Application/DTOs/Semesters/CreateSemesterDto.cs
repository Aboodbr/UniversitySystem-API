using System;

namespace University.Application.DTOs.Semesters;

public class CreateSemesterDto
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}