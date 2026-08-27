using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using University.Application.DTOs;
using University.Application.DTOs.Students;
using University.Application.Interfaces;
using University.Domain.Constants;
using University.Domain.Entities;
using University.Domain.Enums;
using University.Domain.Exceptions;

namespace University.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<EnrollmentResponseDto>> RegisterForCourseAsync(int studentId, int courseOfferingId)
    {
        var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
        if (student == null)
            return new ApiResponse<EnrollmentResponseDto>("Student not found.");

        var offering = await _unitOfWork.CourseOfferings.GetOfferingWithDetailsAsync(courseOfferingId);
        if (offering == null)
            return new ApiResponse<EnrollmentResponseDto>("Course offering not found.");

        // Check if semester is active
        var activeSemester = await _unitOfWork.Semesters.GetActiveSemesterAsync();
        if (activeSemester == null || offering.SemesterId != activeSemester.Id)
            return new ApiResponse<EnrollmentResponseDto>("Cannot enroll in a course for a non-active semester.");

        // Check if student is already enrolled
        if (student.Enrollments.Any(e => e.CourseOfferingId == courseOfferingId && e.Status != EnrollmentStatus.Withdrawn))
            return new ApiResponse<EnrollmentResponseDto>("Student is already enrolled in this course.");

        // Check prerequisites
        if (offering.Course.PrerequisiteCourseId.HasValue)
        {
            var hasPassedPrerequisite = student.Enrollments.Any(e =>
                e.CourseOffering.CourseId == offering.Course.PrerequisiteCourseId.Value &&
                e.Status == EnrollmentStatus.Passed);

            if (!hasPassedPrerequisite)
                return new ApiResponse<EnrollmentResponseDto>($"Student has not passed the prerequisite course.");
        }

        // Check capacity
        var currentEnrollments = await _unitOfWork.Enrollments.GetOfferingsEnrollmentsAsync(courseOfferingId);
        var activeEnrollmentsCount = currentEnrollments.Count(e => e.Status == EnrollmentStatus.Registered || e.Status == EnrollmentStatus.Passed || e.Status == EnrollmentStatus.Failed);
        if (activeEnrollmentsCount >= offering.MaxCapacity)
            return new ApiResponse<EnrollmentResponseDto>("Course offering has reached maximum capacity.");

        // Check max credits per semester
        var currentSemesterEnrollments = student.Enrollments
            .Where(e => e.CourseOffering.SemesterId == activeSemester.Id && e.Status == EnrollmentStatus.Registered)
            .ToList();

        int currentCredits = currentSemesterEnrollments.Sum(e => e.CourseOffering.Course.Credits);
        if (currentCredits + offering.Course.Credits > AcademicConstants.MaxCreditsPerSemester)
            return new ApiResponse<EnrollmentResponseDto>($"Cannot exceed maximum of {AcademicConstants.MaxCreditsPerSemester} credits per semester.");

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseOfferingId = courseOfferingId,
            Status = EnrollmentStatus.Registered
        };

        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.CompleteAsync();

        // Refresh to get full details for DTO
        var createdEnrollment = await _unitOfWork.Enrollments.GetByIdAsync(enrollment.Id);
        // Manual assignment needed because Include on generic GetById is not implemented,
        // but we have it via the related entities for mapping. For a robust app we might query it specifically.
        createdEnrollment.CourseOffering = offering;

        var dto = _mapper.Map<EnrollmentResponseDto>(createdEnrollment);
        return new ApiResponse<EnrollmentResponseDto>(dto, "Successfully registered for course.");
    }

    public async Task<ApiResponse<bool>> DropCourseAsync(int studentId, int courseOfferingId)
    {
        var student = await _unitOfWork.Students.GetStudentWithDetailsAsync(studentId);
        if (student == null)
            return new ApiResponse<bool>("Student not found.");

        var enrollment = student.Enrollments.FirstOrDefault(e => e.CourseOfferingId == courseOfferingId && e.Status == EnrollmentStatus.Registered);
        if (enrollment == null)
            return new ApiResponse<bool>("Active enrollment not found for this course.");

        enrollment.Status = EnrollmentStatus.Withdrawn;
        _unitOfWork.Enrollments.Update(enrollment);
        await _unitOfWork.CompleteAsync();

        return new ApiResponse<bool>(true, "Successfully dropped the course.");
    }
}
