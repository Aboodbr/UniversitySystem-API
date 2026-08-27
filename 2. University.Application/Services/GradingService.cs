using System;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Professors;
using University.Application.Interfaces;
using University.Domain.Enums;
using University.Domain.Exceptions;

namespace University.Application.Services;

public class GradingService : IGradingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGpaService _gpaService;

    public GradingService(IUnitOfWork unitOfWork, IGpaService gpaService)
    {
        _unitOfWork = unitOfWork;
        _gpaService = gpaService;
    }

    public async Task<ApiResponse<bool>> SubmitGradeAsync(SubmitGradeDto submitGradeDto, int professorId)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(submitGradeDto.EnrollmentId);
        if (enrollment == null)
            return new ApiResponse<bool>("Enrollment not found.");

        var offering = await _unitOfWork.CourseOfferings.GetOfferingWithDetailsAsync(enrollment.CourseOfferingId);
        if (offering == null)
            return new ApiResponse<bool>("Course offering not found.");

        if (offering.ProfessorId != professorId)
            return new ApiResponse<bool>("You are not authorized to submit grades for this course.");

        if (enrollment.Status == EnrollmentStatus.Withdrawn)
            return new ApiResponse<bool>("Cannot submit grade for a withdrawn student.");

        // Calculate Grade if not provided explicitly, or validate provided grade
        Grade finalGrade = submitGradeDto.Grade;

        // Simple example calculation based on TotalMarks if provided
        if (submitGradeDto.TotalMarks > 0)
        {
            finalGrade = CalculateGradeFromMarks(submitGradeDto.TotalMarks);
        }

        enrollment.TotalMarks = submitGradeDto.TotalMarks;
        enrollment.Grade = finalGrade;

        enrollment.Status = (finalGrade == Grade.F) ? EnrollmentStatus.Failed : EnrollmentStatus.Passed;

        _unitOfWork.Enrollments.Update(enrollment);
        await _unitOfWork.CompleteAsync();

        // Automatically recalculate student GPA
        await _gpaService.UpdateStudentGpaAsync(enrollment.StudentId);

        return new ApiResponse<bool>(true, "Grade submitted successfully and GPA updated.");
    }

    private Grade CalculateGradeFromMarks(double marks)
    {
        if (marks >= 90) return Grade.A;
        if (marks >= 80) return Grade.B;
        if (marks >= 70) return Grade.C;
        if (marks >= 60) return Grade.D;
        return Grade.F;
    }
}
