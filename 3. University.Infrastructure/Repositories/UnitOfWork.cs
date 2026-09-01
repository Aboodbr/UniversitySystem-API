using System;
using System.Threading.Tasks;
using University.Application.Interfaces;
using University.Application.Interfaces.Repositories;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UniversityDbContext _context;

    public UnitOfWork(UniversityDbContext context)
    {
        _context = context;
        Departments = new DepartmentRepository(_context);
        Students = new StudentRepository(_context);
        Professors = new ProfessorRepository(_context);
        Courses = new CourseRepository(_context);
        CourseOfferings = new CourseOfferingRepository(_context);
        Enrollments = new EnrollmentRepository(_context);
        Semesters = new SemesterRepository(_context);
    }

    public IDepartmentRepository Departments { get; private set; }
    public IStudentRepository Students { get; private set; }
    public IProfessorRepository Professors { get; private set; }
    public ICourseRepository Courses { get; private set; }
    public ICourseOfferingRepository CourseOfferings { get; private set; }
    public IEnrollmentRepository Enrollments { get; private set; }
    public ISemesterRepository Semesters { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
