using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student> GetStudentWithDetailsAsync(int id);
    Task<Student> GetStudentByUserIdAsync(string userId);
}
