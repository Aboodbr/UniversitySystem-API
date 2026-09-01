using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Departments;

namespace University.Application.Interfaces;

public interface IDepartmentService
{
    Task<ApiResponse<IReadOnlyList<DepartmentDto>>> GetAllDepartmentsAsync();
    Task<ApiResponse<DepartmentDto>> GetDepartmentByIdAsync(int id);
    Task<ApiResponse<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto);
    Task<ApiResponse<DepartmentDto>> UpdateDepartmentAsync(UpdateDepartmentDto dto);
    Task<ApiResponse<bool>> DeleteDepartmentAsync(int id);
}