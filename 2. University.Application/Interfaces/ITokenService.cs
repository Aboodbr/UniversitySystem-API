using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(string userId, string email, IList<string> roles);
}
