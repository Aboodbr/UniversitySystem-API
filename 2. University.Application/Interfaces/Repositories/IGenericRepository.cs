using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace University.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync(params Expression<System.Func<T, object>>[] includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
