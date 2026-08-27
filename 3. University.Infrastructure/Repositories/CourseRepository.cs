using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Course>> GetCoursesByDepartmentAsync(int departmentId)
    {
        return await _context.Courses
            .Where(c => c.DepartmentId == departmentId)
            .ToListAsync();
    }
}
