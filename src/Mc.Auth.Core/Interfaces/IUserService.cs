using System.Threading.Tasks;
using Mc.Auth.Core.Entities;

namespace Mc.Auth.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByEmailAsync(string email);
    }
}
