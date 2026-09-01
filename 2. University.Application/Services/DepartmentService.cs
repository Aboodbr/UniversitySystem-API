using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University.Application.DTOs;
using University.Application.DTOs.Departments;
using University.Application.Interfaces;
using University.Domain.Entities;

namespace University.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IReadOnlyList<DepartmentDto>>> GetAllDepartmentsAsync(int pageNumber, int pageSize)
    {
        var departments = await _unitOfWork.Departments.GetPagedAsync(pageNumber, pageSize);
        var departmentDtos = _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
        return new ApiResponse<IReadOnlyList<DepartmentDto>>(departmentDtos);
    }

    public async Task<ApiResponse<DepartmentDetailsDto>> GetDepartmentByIdAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetFirstOrDefaultAsync(d => d.Id == id, d => d.Courses);

        if (department == null)
            return new ApiResponse<DepartmentDetailsDto>("Department not found");

        var departmentDto = _mapper.Map<DepartmentDetailsDto>(department);
        return new ApiResponse<DepartmentDetailsDto>(departmentDto);
    }

    public async Task<ApiResponse<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            Code = dto.Code
        };

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.CompleteAsync();

        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return new ApiResponse<DepartmentDto>(departmentDto, "Department created successfully");
    }

    public async Task<ApiResponse<DepartmentDto>> UpdateDepartmentAsync(UpdateDepartmentDto dto)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(dto.Id);

        if (department == null)
            return new ApiResponse<DepartmentDto>("Department not found");

        department.Name = dto.Name;
        department.Code = dto.Code;

        _unitOfWork.Departments.Update(department);
        await _unitOfWork.CompleteAsync();

        var departmentDto = _mapper.Map<DepartmentDto>(department);
        return new ApiResponse<DepartmentDto>(departmentDto, "Department updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteDepartmentAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);

        if (department == null)
            return new ApiResponse<bool>("Department not found");

        department.IsDeleted = true;
        _unitOfWork.Departments.Update(department);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<bool>(true, "Department deleted successfully");
    }
}
