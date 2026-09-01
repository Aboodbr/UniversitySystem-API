using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Offerings;
using University.Application.DTOs.Students;

namespace University.Application.Interfaces;

public interface IStudentService
{
    Task<ApiResponse<IReadOnlyList<EnrollmentResponseDto>>> GetStudentScheduleAsync(int studentId, int pageNumber, int pageSize);
    Task<ApiResponse<TranscriptDto>> GetStudentTranscriptAsync(int studentId, int pageNumber, int pageSize);
    Task<ApiResponse<StudentProfileDto>> GetStudentProgressAsync(int studentId);
}
