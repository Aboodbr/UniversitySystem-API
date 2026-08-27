using System.Threading.Tasks;
using University.Application.DTOs;

namespace University.Application.Services;

public interface IGpaService
{
    Task<ApiResponse<double>> CalculateGpaAsync(int studentId);
    Task<ApiResponse<bool>> UpdateStudentGpaAsync(int studentId);
}
