using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces.Repositories;

public interface ISemesterRepository : IGenericRepository<Semester>
{
    Task<Semester> GetActiveSemesterAsync();
}
