using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Semesters;
using University.Application.Interfaces;
using University.Domain.Entities;
using University.Domain.Enums;

namespace University.Application.Services;

public class SemesterService : ISemesterService
{
    private readonly IUnitOfWork _unitOfWork;

    public SemesterService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> StartNewSemesterAsync(
        CreateSemesterDto request)
    {
        // Validate dates
        if (request.EndDate <= request.StartDate)
        {
            return new ApiResponse<bool>(
                "End date must be greater than start date."
            );
        }

        // Get the currently active semester
        var currentActiveSemester =
            await _unitOfWork.Semesters.GetActiveSemesterAsync();

        // Complete the current semester
        if (currentActiveSemester != null)
        {
            currentActiveSemester.Status = SemesterStatus.Completed;

            _unitOfWork.Semesters.Update(currentActiveSemester);
        }

        // Create the new semester
        var newSemester = new Semester
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = SemesterStatus.Active
        };

        await _unitOfWork.Semesters.AddAsync(newSemester);

        // Save changes
        var result = await _unitOfWork.CompleteAsync();

        if (result <= 0)
        {
            return new ApiResponse<bool>(
                "Failed to start a new semester. Database error."
            );
        }

        return new ApiResponse<bool>(
            true,
            "New academic semester has been started successfully."
        );
    }
}