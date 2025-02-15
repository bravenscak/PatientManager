using PatientManagerClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(long id);

        Task CreateUserAsync(User user);

        Task UpdateUserAsync(User user);
        
        Task DeleteUserAsync(long id);
    }
}
