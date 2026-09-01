using System.Collections.Generic;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Courses;
using University.Application.DTOs.Offerings;

namespace University.Application.Interfaces;

public interface ICourseService
{
    // Queries (Read Operations)
    Task<ApiResponse<IReadOnlyList<CourseResponseDto>>> GetAllCoursesAsync();
    Task<ApiResponse<CourseResponseDto>> GetCourseByIdAsync(int id);

    // Commands (Write Operations - Used by AdminController)
    Task<ApiResponse<CourseResponseDto>> CreateCourseAsync(CreateCourseDto createCourseDto);
    Task<ApiResponse<CourseOfferingDto>> CreateCourseOfferingAsync(CreateCourseOfferingDto createOfferingDto);
    Task<ApiResponse<CourseResponseDto>> UpdateCourseAsync(int id, UpdateCourseDto dto);
    Task<ApiResponse<bool>> DeleteCourseAsync(int id);
    Task<ApiResponse<IReadOnlyList<CourseOfferingDto>>> GetCourseOfferingsAsync(int courseId, int pageNumber, int pageSize);
    Task<ApiResponse<CourseOfferingDto>> UpdateCourseOfferingAsync(int id, UpdateCourseOfferingDto dto);
}