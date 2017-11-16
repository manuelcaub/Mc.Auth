using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;

namespace Mc.Auth.Core.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool CheckPassword(string password, string email)
        {
            return true;
        }
    }
}
