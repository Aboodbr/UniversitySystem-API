using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces.Repositories;

public interface IProfessorRepository : IGenericRepository<Professor>
{
    Task<Professor> GetProfessorWithDetailsAsync(int id);
    Task<Professor> GetProfessorByUserIdAsync(string userId);
}
