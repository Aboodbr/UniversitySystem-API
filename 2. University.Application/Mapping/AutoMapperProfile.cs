using AutoMapper;
using University.Domain.Entities;
using University.Application.DTOs.Students;
using University.Application.DTOs.Courses;
using University.Application.DTOs.Professors;
using University.Application.DTOs.Offerings;
using University.Application.DTOs.Departments;

namespace University.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Student Mapping
        CreateMap<Student, StudentResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.AcademicStatus, opt => opt.MapFrom(src => src.AcademicStatus.ToString()));

        CreateMap<Student, StudentProfileDto>()
            .IncludeBase<Student, StudentResponseDto>();

        // Course Mapping
        CreateMap<Course, CourseResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<CreateCourseDto, Course>();

        // Professor Mapping
        CreateMap<Professor, ProfessorResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        // Course Offering Mapping
        CreateMap<CourseOffering, CourseOfferingDto>()
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.Code))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester.Name))
            .ForMember(dest => dest.ProfessorName, opt => opt.MapFrom(src => src.Professor.FirstName + " " + src.Professor.LastName))
            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.BuildingName + " " + src.Room.RoomNumber));

        CreateMap<CreateCourseOfferingDto, CourseOffering>();

        // Department Mapping
        CreateMap<Department, DepartmentDto>();

        // Enrollment Mapping
        CreateMap<Enrollment, EnrollmentResponseDto>()
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseOffering.Course.Code))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.CourseOffering.Course.Title))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.HasValue ? src.Grade.ToString() : null));
    }
}
