using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Professors;

namespace University.Application.Services;

public interface IGradingService
{
    Task<ApiResponse<bool>> SubmitGradeAsync(SubmitGradeDto submitGradeDto, int professorId);
}
