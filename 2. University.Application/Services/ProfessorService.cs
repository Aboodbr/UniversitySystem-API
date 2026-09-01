using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University.Application.DTOs;
using University.Application.DTOs.Offerings;
using University.Application.DTOs.Professors;
using University.Application.DTOs.Students;
using University.Application.Interfaces;

namespace University.Application.Services;

public class ProfessorService : IProfessorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProfessorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IReadOnlyList<ProfessorResponseDto>>> GetAllProfessorsAsync(int pageNumber, int pageSize)
    {
        var professors = await _unitOfWork.Professors.GetPagedAsync(pageNumber, pageSize, p => p.Department);
        var mapped = _mapper.Map<IReadOnlyList<ProfessorResponseDto>>(professors);
        return new ApiResponse<IReadOnlyList<ProfessorResponseDto>>(mapped);
    }

    public async Task<ApiResponse<IReadOnlyList<CourseOfferingDto>>> GetProfessorScheduleAsync(int professorId, int pageNumber, int pageSize)
    {
        var offerings = await _unitOfWork.CourseOfferings.GetPagedAsync(o => o.ProfessorId == professorId, pageNumber, pageSize, o => o.Course, o => o.Semester, o => o.Professor, o => o.Room);
        var mapped = _mapper.Map<IReadOnlyList<CourseOfferingDto>>(offerings);
        return new ApiResponse<IReadOnlyList<CourseOfferingDto>>(mapped);
    }

    public async Task<ApiResponse<IReadOnlyList<StudentResponseDto>>> GetOfferingStudentsAsync(int offeringId, int pageNumber, int pageSize)
    {
        var enrollments = await _unitOfWork.Enrollments.GetPagedAsync(e => e.CourseOfferingId == offeringId, pageNumber, pageSize, e => e.Student, e => e.Student.Department);
        var students = enrollments.Select(e => e.Student).ToList();
        var mapped = _mapper.Map<IReadOnlyList<StudentResponseDto>>(students);
        return new ApiResponse<IReadOnlyList<StudentResponseDto>>(mapped);
    }
}
