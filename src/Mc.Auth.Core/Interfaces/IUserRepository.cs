using System.Threading.Tasks;
using Mc.Auth.Core.Entities;

namespace Mc.Auth.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);

        Task AddUserAsync(User user);
    }
}
