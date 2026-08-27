using System.Collections.Generic;
using System.Threading.Tasks;
using University.Domain.Entities;

namespace University.Application.Interfaces.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<IReadOnlyList<Enrollment>> GetStudentEnrollmentsAsync(int studentId);
    Task<IReadOnlyList<Enrollment>> GetOfferingsEnrollmentsAsync(int courseOfferingId);
}
