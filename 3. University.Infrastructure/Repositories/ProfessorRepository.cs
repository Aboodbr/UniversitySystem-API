using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class ProfessorRepository : GenericRepository<Professor>, IProfessorRepository
{
    public ProfessorRepository(UniversityDbContext context) : base(context)
    {
    }

    public async Task<Professor> GetProfessorByUserIdAsync(string userId)
    {
        return await _context.Professors
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Professor> GetProfessorWithDetailsAsync(int id)
    {
        return await _context.Professors
            .Include(p => p.Department)
            .Include(p => p.CourseOfferings)
                .ThenInclude(co => co.Course)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
