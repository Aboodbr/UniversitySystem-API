using System;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Domain.Enums;

namespace University.Application.Services;

public class GpaService : IGpaService
{
    private readonly IUnitOfWork _unitOfWork;

    public GpaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GPA = SUM OF (CREDITS X GRADE POINT)/SUM OF CREDITS
    public async Task<ApiResponse<double>> CalculateGpaAsync(int studentId)
    {
        var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsAsync(studentId);

        var completedCourses = enrollments
            .Where(e => e.Grade.HasValue && e.Grade != Grade.W && e.Grade != Grade.I)
            .ToList();

        if (!completedCourses.Any())
            return new ApiResponse<double>(0.0, "No completed courses found.");

        double totalGradePoints = 0;
        int totalCredits = 0;

        foreach (var enrollment in completedCourses)
        {
            var credits = enrollment.CourseOffering.Course.Credits;
            totalCredits += credits;
            totalGradePoints += GetGradePoints(enrollment.Grade.Value) * credits;
        }

        var gpa = totalCredits > 0 ? Math.Round(totalGradePoints / totalCredits, 2) : 0.0;

        return new ApiResponse<double>(gpa, "GPA calculated successfully.");
    }

    public async Task<ApiResponse<bool>> UpdateStudentGpaAsync(int studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            return new ApiResponse<bool>("Student not found.");

        var gpaResult = await CalculateGpaAsync(studentId);
        if (!gpaResult.Success)
            return new ApiResponse<bool>(gpaResult.Message);

        student.GPA = gpaResult.Data;

        // Update completed hours
        var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsAsync(studentId);
        student.CompletedHours = enrollments
            .Where(e => e.Status == EnrollmentStatus.Passed)
            .Sum(e => e.CourseOffering.Course.Credits);

        _unitOfWork.Students.Update(student);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<bool>(true, "Student GPA and completed hours updated successfully.");
    }

    private double GetGradePoints(Grade grade)
    {
        return grade switch
        {
            Grade.A => 4.0,
            Grade.B => 3.0,
            Grade.C => 2.0,
            Grade.D => 1.0,
            Grade.F => 0.0,
            _ => 0.0
        };
    }
}
