using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Domain.Enums;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
{
    public SemesterRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<Semester> GetActiveSemesterAsync()
    {
        return await _context.Semesters.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Status == SemesterStatus.Active);
    }
}
