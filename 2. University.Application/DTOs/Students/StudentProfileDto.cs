using System.Collections.Generic;

namespace University.Application.DTOs.Students;

public class StudentProfileDto : StudentResponseDto
{
    public List<EnrollmentResponseDto> CurrentEnrollments { get; set; } = new List<EnrollmentResponseDto>();
}
