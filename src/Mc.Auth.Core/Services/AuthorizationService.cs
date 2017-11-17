using System.Threading.Tasks;
using Mc.Auth.Core.Interfaces;

namespace Mc.Auth.Core.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckPasswordAsync(string password, string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            return !string.IsNullOrWhiteSpace(user?.Password)
                && user.Password == password;
        }
    }
}
