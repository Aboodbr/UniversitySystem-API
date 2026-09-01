using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Offerings;
using University.Application.DTOs.Professors;
using University.Application.DTOs.Students;

namespace University.Application.Interfaces;

public interface IProfessorService
{
    Task<ApiResponse<IReadOnlyList<ProfessorResponseDto>>> GetAllProfessorsAsync(int pageNumber, int pageSize);
    Task<ApiResponse<IReadOnlyList<CourseOfferingDto>>> GetProfessorScheduleAsync(int professorId, int pageNumber, int pageSize);
    Task<ApiResponse<IReadOnlyList<StudentResponseDto>>> GetOfferingStudentsAsync(int offeringId, int pageNumber, int pageSize);
}
