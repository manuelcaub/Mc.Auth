using System.Threading.Tasks;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;

namespace Mc.Auth.Core.Services
{
    public class UserService : IUserService
    {
        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult(new User());
        }
    }
}
