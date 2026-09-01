using System;
using System.Threading.Tasks;
using University.Application.Interfaces.Repositories;

namespace University.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDepartmentRepository Departments { get; }
    IStudentRepository Students { get; }
    IProfessorRepository Professors { get; }
    ICourseRepository Courses { get; }
    ICourseOfferingRepository CourseOfferings { get; }
    IEnrollmentRepository Enrollments { get; }
    ISemesterRepository Semesters { get; }

    Task<int> CompleteAsync();
}
