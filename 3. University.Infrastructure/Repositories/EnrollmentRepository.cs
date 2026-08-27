using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Enrollment>> GetOfferingsEnrollmentsAsync(int courseOfferingId)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Where(e => e.CourseOfferingId == courseOfferingId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(int studentId)
    {
        return await _context.Enrollments
            .Include(e => e.CourseOffering)
                .ThenInclude(co => co.Course)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }
}
