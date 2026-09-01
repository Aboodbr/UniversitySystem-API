using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Semesters;

namespace University.Application.Services;

public interface ISemesterService
{
    Task<ApiResponse<bool>> StartNewSemesterAsync(
        CreateSemesterDto request);
}