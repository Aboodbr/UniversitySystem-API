using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces.Repositories;

public interface ICourseOfferingRepository : IGenericRepository<CourseOffering>
{
    Task<IReadOnlyList<CourseOffering>> GetActiveOfferingsBySemesterAsync(int semesterId);
    Task<CourseOffering> GetOfferingWithDetailsAsync(int id);
}
