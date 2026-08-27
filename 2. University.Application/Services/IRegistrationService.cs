using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Students;

namespace University.Application.Services;

public interface IRegistrationService
{
    Task<ApiResponse<EnrollmentResponseDto>> RegisterForCourseAsync(int studentId, int courseOfferingId);
    Task<ApiResponse<bool>> DropCourseAsync(int studentId, int courseOfferingId);
}
