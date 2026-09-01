using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using University.Application.DTOs;
using University.Application.DTOs.Courses;
using University.Application.DTOs.Offerings;
using University.Application.Interfaces;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities; 

namespace University.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IReadOnlyList<CourseResponseDto>>> GetAllCoursesAsync()
    {
        // 1. جلب البيانات من قاعدة البيانات
        var courses = await _unitOfWork.Courses.GetAllAsync(c => c.Department);

        // 2. تحويلها إلى DTOs
        var data = _mapper.Map<IReadOnlyList<CourseResponseDto>>(courses);

        return new ApiResponse<IReadOnlyList<CourseResponseDto>>(data, "Courses retrieved successfully.");
    }

    public async Task<ApiResponse<CourseResponseDto>> GetCourseByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);

        if (course == null)
            return new ApiResponse<CourseResponseDto>("Course not found.");

        var data = _mapper.Map<CourseResponseDto>(course);
        return new ApiResponse<CourseResponseDto>(data, "Course retrieved successfully.");
    }

    public async Task<ApiResponse<CourseResponseDto>> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        // 1. تحويل الـ DTO القادم من المستخدم إلى Entity
        var courseEntity = _mapper.Map<Course>(createCourseDto);

        // 2. إضافته إلى الـ Repository
        await _unitOfWork.Courses.AddAsync(courseEntity);

        // 3. حفظ التغييرات في قاعدة البيانات
        var result = await _unitOfWork.CompleteAsync();
        if (result <= 0)
            return new ApiResponse<CourseResponseDto>("Failed to create the course.");

        var data = _mapper.Map<CourseResponseDto>(courseEntity);
        return new ApiResponse<CourseResponseDto>(data, "Course created successfully.");
    }

    public async Task<ApiResponse<CourseOfferingDto>> CreateCourseOfferingAsync(CreateCourseOfferingDto createOfferingDto)
    {
        var offeringEntity = _mapper.Map<CourseOffering>(createOfferingDto);

        

        var result = await _unitOfWork.CompleteAsync();
        if (result <= 0)
            return new ApiResponse<CourseOfferingDto>("Failed to create the course offering.");

        var data = _mapper.Map<CourseOfferingDto>(offeringEntity);
        return new ApiResponse<CourseOfferingDto>(data, "Course offering created successfully.");
    }
}