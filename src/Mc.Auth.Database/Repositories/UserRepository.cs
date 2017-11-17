using System.Linq;
using System.Threading.Tasks;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Mc.Auth.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Mc.Auth.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.Where(user => user.UserName == email).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
