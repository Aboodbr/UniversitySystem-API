using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<Student> GetStudentByUserIdAsync(string userId)
    {
        return await _context.Students
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task<Student> GetStudentWithDetailsAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Department)
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.CourseOffering)
                    .ThenInclude(co => co.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
