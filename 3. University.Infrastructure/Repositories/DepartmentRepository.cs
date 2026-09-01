using University.Application.Interfaces.Repositories;
using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(UniversityDbContext context) : base(context)
    {
    }
}
