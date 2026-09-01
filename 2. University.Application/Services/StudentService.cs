using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University.Application.DTOs;
using University.Application.DTOs.Students;
using University.Application.Interfaces;

namespace University.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IReadOnlyList<EnrollmentResponseDto>>> GetStudentScheduleAsync(int studentId, int pageNumber, int pageSize)
    {
        var active = await _unitOfWork.Enrollments.GetPagedAsync(e => e.StudentId == studentId && e.Status == Domain.Enums.EnrollmentStatus.Registered, pageNumber, pageSize, e => e.CourseOffering, e => e.CourseOffering.Course);
        var mapped = _mapper.Map<IReadOnlyList<EnrollmentResponseDto>>(active);
        return new ApiResponse<IReadOnlyList<EnrollmentResponseDto>>(mapped);
    }

    public async Task<ApiResponse<TranscriptDto>> GetStudentTranscriptAsync(int studentId, int pageNumber, int pageSize)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null) return new ApiResponse<TranscriptDto>("Student not found.");

        var completed = await _unitOfWork.Enrollments.GetPagedAsync(e => e.StudentId == studentId && (e.Status == Domain.Enums.EnrollmentStatus.Passed || e.Status == Domain.Enums.EnrollmentStatus.Failed), pageNumber, pageSize, e => e.CourseOffering, e => e.CourseOffering.Course);

        var transcriptDto = new TranscriptDto
        {
            StudentName = $"{student.FirstName} {student.LastName}",
            OverallGPA = student.GPA,
            TotalCompletedHours = student.CompletedHours,
            CompletedCourses = _mapper.Map<List<EnrollmentResponseDto>>(completed)
        };
        return new ApiResponse<TranscriptDto>(transcriptDto);
    }

    public async Task<ApiResponse<StudentProfileDto>> GetStudentProgressAsync(int studentId)
    {
        var student = await _unitOfWork.Students.GetFirstOrDefaultAsync(s => s.Id == studentId, s => s.Department);
        if (student == null) return new ApiResponse<StudentProfileDto>("Student not found.");

        var mapped = _mapper.Map<StudentProfileDto>(student);
        return new ApiResponse<StudentProfileDto>(mapped);
    }
}
