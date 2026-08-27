using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class CourseOfferingRepository : GenericRepository<CourseOffering>, ICourseOfferingRepository
{
    public CourseOfferingRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<CourseOffering>> GetActiveOfferingsBySemesterAsync(int semesterId)
    {
        return await _context.CourseOfferings
            .Include(co => co.Course)
            .Include(co => co.Professor)
            .Include(co => co.Room)
            .Where(co => co.SemesterId == semesterId)
            .ToListAsync();
    }

    public async Task<CourseOffering> GetOfferingWithDetailsAsync(int id)
    {
        return await _context.CourseOfferings
            .Include(co => co.Course)
            .Include(co => co.Professor)
            .Include(co => co.Room)
            .Include(co => co.Semester)
            .FirstOrDefaultAsync(co => co.Id == id);
    }
}
