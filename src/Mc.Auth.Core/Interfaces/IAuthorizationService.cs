using System.Threading.Tasks;

namespace Mc.Auth.Core.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> CheckPasswordAsync(string password, string email);
    }
}
