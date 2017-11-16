namespace Mc.Auth.Core.Interfaces
{
    public interface IAuthorizationService
    {
        bool CheckPassword(string password, string email);
    }
}
